using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Webhooks.Models.Payload;

namespace Apps.Asana.Webhooks;

[WebhookList]
public class WebhookList
{
    const string SecretHeaderKey = "X-Hook-Secret";

    [Webhook("On project changed", typeof(ProjectChangedHandler),
        Description = "Triggered when changes are made to the project")]
    public async Task<WebhookResponse<ProjectDto>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.ContainsKey(SecretHeaderKey))
        {
            var responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            responseMessage.Headers.Add(SecretHeaderKey, webhookRequest.Headers[SecretHeaderKey]);
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = responseMessage,
                Result = null
            };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());

        return data is not null
            ? new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            }
            : throw new InvalidCastException(nameof(webhookRequest.Body));
    }
}