using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Webhooks.Models.Request;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Asana.Webhooks.Handlers;

public class BaseWebhookHandler : IWebhookEventHandler
{
    private readonly string _resourceId;
    private readonly string _resourceType;
    private readonly string _action;
    private readonly string _subType;
    private readonly AsanaClient _client;

    public BaseWebhookHandler(string resourceId, string resourceType, string action, string subType = null)
    {
        _resourceId = resourceId;
        _resourceType = resourceType;
        _action = action;
        _subType = subType;

        _client = new();
    }

    public async Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        try
        {
            var obj = new
            {
                data = new
                {
                    resource = _resourceId,
                    target = values["payloadUrl"],
                    filters = new[]
                    {
                        new
                        {
                            action = _action,
                            resource_type = _resourceType,
                            resource_subtype = _subType,
                        }
                    }
                }
            };
            
            await Logger.LogAsync(new
            {
                ResourceId = _resourceId,
                ResourceType = _resourceType,
                Action = _action,
                Values = values,
                Creds = creds,
                Obj = obj
            });
            
            var request = new AsanaRequest(ApiEndpoints.Webhooks, Method.Post, creds)
                .WithJsonBody(obj);
            
            await _client.ExecuteWithErrorHandling(request);
        }
        catch (Exception e)
        {
            await Logger.LogException(e);
            throw;
        }
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var webhooks = await GetAllWebhooks(creds);
        var webhookGId = webhooks.FirstOrDefault()?.Gid;

        if (webhookGId is null)
            return;

        var endpoint = $"{ApiEndpoints.Webhooks}/{webhookGId}";
        var request = new AsanaRequest(endpoint, Method.Delete, creds);

        await _client.ExecuteWithErrorHandling(request);
    }

    private Task<IEnumerable<AsanaEntity>> GetAllWebhooks(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var endpoint = $"{ApiEndpoints.Webhooks}/{_resourceId}";
        var request = new AsanaRequest(endpoint, Method.Get, creds);

        return _client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);
    }
}