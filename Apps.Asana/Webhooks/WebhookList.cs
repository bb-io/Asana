using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    const string SecretHeaderKey = "X-Hook-Secret";

    [Webhook("On project changed", typeof(ProjectChangedHandler),
        Description = "Triggered when changes are made to the project")]
    public async Task<WebhookResponse<ProjectDto>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
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
                    Result = null
                };
            }

            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString()!);
            return data is not null
                ? new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = null,
                    Result = data.Project
                }
                : throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        catch (Exception e)
        {
            InvocationContext?.Logger?.LogError($"Error occurred while processing webhook request: {e.Message}; " +
                                                $"JSON: {webhookRequest.Body}; Headers: {JsonConvert.SerializeObject(webhookRequest.Headers)}", null);
            throw;
        }
    }
}