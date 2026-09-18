using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Asana.Webhooks.Handlers;

public class BaseWebhookHandler : IWebhookEventHandler, IAsyncValidatableWebhookEventHandler
{
    private readonly string _resourceId;
    private readonly string _resourceType;
    private readonly string _action;
    private readonly string? _resourceSubType;
    private readonly string? _workspaceId;
    private readonly AsanaClient _client;

    public BaseWebhookHandler(string resourceId, string resourceType, string action, string? resourceSubType = null, string? workspaceId = null)
    {
        _resourceId = resourceId;
        _resourceType = resourceType;
        _action = action;
        _resourceSubType = resourceSubType;

        _client = new();
        _workspaceId = workspaceId;
    }

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var credsList = creds.ToList();
        
        string target = values["payloadUrl"];
        var desiredFilter = BuildFilter();

        var allWebhooks = await GetAllWebhooks(credsList, values);
        var existing = allWebhooks.FirstOrDefault(w => string.Equals(w.Target, target, StringComparison.OrdinalIgnoreCase));
        
        if (existing?.Active == false)
        {
            await DeleteWebhook(credsList, existing.Gid);
            existing = null;
        }
        
        if (existing is null)
        {
            await CreateWebhook(credsList, target, new[] { desiredFilter });
            return;
        }

        if (existing.Filters?.Any(f => FilterEquals(f, desiredFilter)) == true)
            return;

        var merged = (existing.Filters ?? new List<Dictionary<string, object>>())
            .Concat(new[] { desiredFilter })
            .ToArray();

        await DeleteWebhook(credsList, existing.Gid);
        await CreateWebhook(credsList, target, merged);
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

    public async Task<WebhookSubscriptionValidationResponse> ValidateSubscription(
        IEnumerable<AuthenticationCredentialsProvider> creds, 
        Dictionary<string, string> values)
    {
        string target = values["payloadUrl"];

        IEnumerable<WebhookSubscription> allWebhooks;
        try
        {
            allWebhooks = await GetAllWebhooks(creds, values);
        }
        catch
        {
            // Couldn't list all webhooks != something's wrong with the subscription (like a temporary 500 error)
            return new() { IsValid = true };
        }
        
        var existing = allWebhooks.FirstOrDefault(w => string.Equals(w.Target, target, StringComparison.OrdinalIgnoreCase));
        if (existing is null)
        {
            return new()
            {
                IsValid = false,
                Message = "The Asana webhook for this event no longer exists. Reactivate the bird to recreate it"
            };
        }

        if (!existing.Active)
        {
            return new()
            {
                IsValid = false,
                Message = "The Asana webhook for this event is inactive. Reactivate the bird to create a new active one"
            };
        }

        return new() { IsValid = true };
    }

    protected Dictionary<string, object> BuildFilter()
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

    protected static bool FilterEquals(Dictionary<string, object> a, Dictionary<string, object> b)
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

    public async Task<IEnumerable<WebhookSubscription>> GetAllWebhooks(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var workspaceId = _workspaceId;

        if (string.IsNullOrWhiteSpace(workspaceId) && values != null)
        {
            var caseInsensitive = new Dictionary<string, string>(values, StringComparer.OrdinalIgnoreCase);

            if (caseInsensitive.TryGetValue("workspaceId", out var v1)) workspaceId = v1;
            else if (caseInsensitive.TryGetValue("workspaceGid", out var v2)) workspaceId = v2;
            else if (caseInsensitive.TryGetValue("workspace", out var v3)) workspaceId = v3;
        }

        if (string.IsNullOrWhiteSpace(workspaceId))
            throw new Exception("workspaceId is required for listing webhooks (not provided to handler constructor).");

        var endpoint = $"{ApiEndpoints.Webhooks}?workspace={workspaceId}&resource={_resourceId}";
        var request = new AsanaRequest(endpoint, Method.Get, creds).AddQueryParameter("opt_fields", "filters,active");

        return await _client.Paginate<WebhookSubscription>(request);
    }
}