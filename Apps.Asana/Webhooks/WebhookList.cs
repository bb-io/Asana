using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Apps.Asana.Webhooks
{
    [WebhookList]
    public class WebhookList
    {
        const string SecretHeaderKey = "X-Hook-Secret";

        [Webhook("On project changed", typeof(ProjectChangedHandler), Description = "Triggered when changes are made to the project")]
        public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest)
        {
            if (webhookRequest.Headers.ContainsKey(SecretHeaderKey))
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
                httpResponseMessage.Headers.Add(SecretHeaderKey, webhookRequest.Headers[SecretHeaderKey]);
                return new WebhookResponse<ProjectDto>
                {
                    HttpResponseMessage = httpResponseMessage,
                    Result = null
                };
            }

            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        public class ProjectWrapper
        {
            public ProjectDto Project { get; set; }
        }
    }
}
