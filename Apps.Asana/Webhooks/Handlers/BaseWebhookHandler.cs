using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.Models;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana.Webhooks.Handlers
{
    public class BaseWebhookHandler : IWebhookEventHandler
    {
        private string ResourceId;
        private string ResourceType;
        private string Action;
        public BaseWebhookHandler(string resourceId, string resourceType, string action)
        {
            ResourceId = resourceId;
            ResourceType = resourceType;
            Action = action;
        }
        public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var client = new AsanaClient();
            var request = new AsanaWebhookRequest($"/webhooks", Method.Post, authenticationCredentialsProvider);
            request.AddJsonBody(new
            {
                resource = ResourceId,
                target = values["payloadUrl"],
                filters = new[] { new { action = Action, resource_type = ResourceType }}
            });

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                client.Execute(request);
            });
        }

        public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var client = new AsanaClient();
            var getRequest = new AsanaWebhookRequest($"/webhooks/{ResourceId}", Method.Get, authenticationCredentialsProvider);
            var webhooks = await client.GetAsync<ResponseWrapper<List<WebhookDto>>>(getRequest);
            var webhookGId = webhooks.Data.First().GId;

            var deleteRequest = new AsanaRequest($"/webhooks/{webhookGId}", Method.Delete, authenticationCredentialsProvider);
            await client.ExecuteAsync(deleteRequest);
        }
    }
}
