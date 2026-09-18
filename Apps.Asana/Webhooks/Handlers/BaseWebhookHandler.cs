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
        var existingWebhooks = await FindByTarget(credsList, values, target);

        // Asana rejects a second webhook on the same resource + target
        if (existingWebhooks.Count > 0)
            return;

        await CreateWebhook(credsList, target);
    }

    public async Task UnsubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var credsList = creds.ToList();
        
        string target = values["payloadUrl"];
        var existingWebhooks = await FindByTarget(credsList, values, target);

        foreach (var webhook in existingWebhooks)
            await DeleteWebhook(credsList, webhook.Gid);
    }

    public async Task<WebhookSubscriptionValidationResponse> ValidateSubscription(
        IEnumerable<AuthenticationCredentialsProvider> creds, 
        Dictionary<string, string> values)
    {
        string target = values["payloadUrl"];
        
        var existingWebhooks = await FindByTarget(creds, values, target);
        bool activeWebhookExists = existingWebhooks.Any(x => x.Active);

        if (activeWebhookExists)
            return new() { IsValid = true };

        return new()
        {
            IsValid = false,
            Message = "No active subscription was found for this bird in Asana. Please republish it"
        };
    }
    
    private async Task<List<WebhookSubscription>> FindByTarget(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values,
        string target)
    {
        var webhooks = await GetAllWebhooks(creds, values);
        return webhooks
            .Where(w => string.Equals(w.Target.TrimEnd('/'), target.TrimEnd('/'), StringComparison.OrdinalIgnoreCase))
            .ToList();
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

    private async Task CreateWebhook(IEnumerable<AuthenticationCredentialsProvider> creds, string target)
    {
        var data = new Dictionary<string, object>
        {
            ["resource"] = _resourceId,
            ["target"] = target,
            ["filters"] = new[] { BuildFilter() }
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
        var request = new AsanaRequest(endpoint, Method.Get, creds);

        return await _client.ExecuteWithErrorHandling<List<WebhookSubscription>>(request);
    }
}