using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Actions;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Webhooks.Models.Payload;
using Apps.Asana.Webhooks.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    const string SecretHeaderKey = "X-Hook-Secret";

    [Webhook("On projects changed", typeof(ProjectChangedHandler),
        Description = "Triggered when changes are made to projects")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<ProjectChangedPayload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse();
        }

        var projects = await GetProjectsFromPayload(payload);
        return new WebhookResponse<ProjectsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new ProjectsResponse { Projects = projects },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    private WebhookResponse<ProjectsResponse> CreatePreflightResponse(string? secretKey = null)
    {
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        if (!string.IsNullOrEmpty(secretKey))
        {
            responseMessage.Headers.Add(SecretHeaderKey, secretKey);
        }

        return new WebhookResponse<ProjectsResponse>
        {
            HttpResponseMessage = responseMessage,
            Result = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
    }

    private async Task<List<ProjectDto>> GetProjectsFromPayload(ProjectChangedPayload payload)
    {
        var projectActions = new ProjectActions(InvocationContext);
        var projects = new List<ProjectDto>();

        foreach (var project in payload.Events.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var projectDto = await projectActions.GetProject(new ProjectRequest
            {
                ProjectId = project.Resource.Gid
            });
            projects.Add(projectDto);
        }

        return projects;
    }
}