using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Logging;

namespace Apps.Asana.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    const string SecretHeaderKey = "X-Hook-Secret";

    [Webhook("On project changed", typeof(ProjectChangedHandler),
        Description = "Triggered when changes are made to the project")]
    public async Task<WebhookResponse<ProjectDto>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
        await Logger.LogAsync(new
        {
            webhookRequest.Body,
            webhookRequest.Headers,
        });
        
        try
        {
            if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
            {
                var responseMessage = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
                responseMessage.Headers.Add(SecretHeaderKey, secretKey);
                
                return new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = responseMessage,
                    Result = null,
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                };
            }

            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString()!);
            return data is not null
                ? new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = null,
                    Result = data.Project, 
                }
                : throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        catch (Exception e)
        {
            await Logger.LogException(e);
            throw;
        }
    }
    
    [Webhook("Test webhook",
        Description = "Triggered when changes are made to the project")]
    public async Task<WebhookResponse<ProjectDto>> TestWebhookHandler(WebhookRequest webhookRequest)
    {
        await Logger.LogAsync(new
        {
            webhookRequest.Body,
            webhookRequest.Headers,
        });
        
        try
        {
            if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
            {
                var responseMessage = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
                
                responseMessage.Content.Headers.Add(SecretHeaderKey, secretKey);
                responseMessage.Headers.Add(SecretHeaderKey, secretKey);
                
                await Logger.LogAsync(new
                {
                    responseMessage,
                    secretKey
                });
                
                return new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = responseMessage,
                    Result = null,
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                };
            }

            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString()!);
            return data is not null
                ? new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = null,
                    Result = data.Project, 
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                }
                : throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        catch (Exception e)
        {
            await Logger.LogException(e);
            throw;
        }
    }
}