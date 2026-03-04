using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Asana.Webhooks.Handlers;

public class BaseWebhookHandler : IWebhookEventHandler
{
    private readonly string _resourceId;
    private readonly string _resourceType;
    private readonly string _action;
    private readonly string? _resourceSubType;
    private readonly AsanaClient _client;

    public BaseWebhookHandler(string resourceId, string resourceType, string action, string? resourceSubType = null)
    {
        _resourceId = resourceId;
        _resourceType = resourceType;
        _action = action;
        _resourceSubType = resourceSubType;

        _client = new();
    }

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var target = values["payloadUrl"];

        var desiredFilter = BuildFilter();

        var existing = (await GetAllWebhooks(creds, values))
    .FirstOrDefault(w => string.Equals(w.Target, target, StringComparison.OrdinalIgnoreCase));

        if (existing is null)
        {
            await CreateWebhook(creds, target, new[] { desiredFilter });
            return;
        }

        if (existing.Filters?.Any(f => FilterEquals(f, desiredFilter)) == true)
            return;

        var merged = (existing.Filters ?? new List<Dictionary<string, object>>())
            .Concat(new[] { desiredFilter })
            .ToArray();

        await DeleteWebhook(creds, existing.Gid);
        await CreateWebhook(creds, target, merged);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var target = values["payloadUrl"];
        var filterToRemove = BuildFilter();

        var existing = (await GetAllWebhooks(creds, values))
                .FirstOrDefault(w => string.Equals(w.Target, target, StringComparison.OrdinalIgnoreCase));

        if (existing is null)
            return;

        var remaining = (existing.Filters ?? new List<Dictionary<string, object>>())
             .Where(f => !FilterEquals(f, filterToRemove))
             .ToArray();

        if (!remaining.Any())
        {
            await DeleteWebhook(creds, existing.Gid);
            return;
        }

        await DeleteWebhook(creds, existing.Gid);
        await CreateWebhook(creds, target, remaining);
    }

    private Dictionary<string, object> BuildFilter()
    {
        var filter = new Dictionary<string, object>
        {
            ["action"] = _action,
            ["resource_type"] = _resourceType
        };

        if (!string.IsNullOrEmpty(_resourceSubType))
            filter["resource_subtype"] = _resourceSubType;

        return filter;
    }

    private static bool FilterEquals(Dictionary<string, object> a, Dictionary<string, object> b)
    {
        string? Get(Dictionary<string, object> d, string k) => d.TryGetValue(k, out var v) ? v?.ToString() : null;

        return string.Equals(Get(a, "action"), Get(b, "action"), StringComparison.OrdinalIgnoreCase)
            && string.Equals(Get(a, "resource_type"), Get(b, "resource_type"), StringComparison.OrdinalIgnoreCase)
            && string.Equals(Get(a, "resource_subtype"), Get(b, "resource_subtype"), StringComparison.OrdinalIgnoreCase);
    }

    private async Task CreateWebhook(IEnumerable<AuthenticationCredentialsProvider> creds, string target,
        IEnumerable<Dictionary<string, object>> filters)
    {
        var data = new Dictionary<string, object>
        {
            ["resource"] = _resourceId,
            ["target"] = target,
            ["filters"] = filters.ToArray()
        };

        var request = new AsanaRequest(ApiEndpoints.Webhooks, Method.Post, creds)
            .WithJsonBody(new Dictionary<string, object> { ["data"] = data });

        await _client.ExecuteWithErrorHandling(request);
    }

    private async Task DeleteWebhook(IEnumerable<AuthenticationCredentialsProvider> creds, string webhookGid)
    {
        var request = new AsanaRequest($"{ApiEndpoints.Webhooks}/{webhookGid}", Method.Delete, creds);
        await _client.ExecuteWithErrorHandling(request);
    }

    private async Task<IEnumerable<WebhookSubscription>> GetAllWebhooks(
    IEnumerable<AuthenticationCredentialsProvider> creds,
    Dictionary<string, string> values)
    {
        var endpoint = $"{ApiEndpoints.Webhooks}?resource={_resourceId}";

        if (values.TryGetValue("workspace", out var workspace) && !string.IsNullOrWhiteSpace(workspace))
            endpoint += $"&workspace={workspace}";

        var request = new AsanaRequest(endpoint, Method.Get, creds);

        var response = await _client.ExecuteWithErrorHandling<ListResponse<WebhookSubscription>>(request);
        return response.Data;
    }
}