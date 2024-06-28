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
    
    #region Projects
    
    [Webhook("On projects added", typeof(ProjectsAddedHandler),
        Description = "Triggered when added are made to projects")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
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

    [Webhook("On projects changed", typeof(ProjectChangedHandler),
        Description = "Triggered when changes are made to projects")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
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
    
    [Webhook("On projects deleted", typeof(ProjectDeletedHandler),
        Description = "Triggered when projects are deleted")]
    public async Task<WebhookResponse<ProjectsDeletedResponse>> ProjectsDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectsDeletedResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectsDeletedResponse>();
        }

        return new WebhookResponse<ProjectsDeletedResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new ProjectsDeletedResponse 
            { 
                DeletedProjectIds = payload.Events
                    .Where(x => x.Action == "deleted" &&  x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }
    
    [Webhook("On projects removed", typeof(ProjectRemovedHandler),
        Description = "Triggered when projects are removed")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsRemovedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
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
    
    [Webhook("On projects undeleted", typeof(ProjectUndeletedHandler),
        Description = "Triggered when projects are undeleted")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsuUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });
        
        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
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
    
    #endregion
    
    #region Utils

    private WebhookResponse<T> CreatePreflightResponse<T>(string? secretKey = null)
        where T : class
    {
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        if (!string.IsNullOrEmpty(secretKey))
        {
            responseMessage.Headers.Add(SecretHeaderKey, secretKey);
        }

        return new WebhookResponse<T>
        {
            HttpResponseMessage = responseMessage,
            Result = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
    }

    private async Task<List<ProjectDto>> GetProjectsFromPayload(Payload payload)
    {
        var projectActions = new ProjectActions(InvocationContext);
        var projects = new List<ProjectDto>();

        foreach (var project in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var projectDto = await projectActions.GetProject(new ProjectRequest
            {
                ProjectId = project.Resource.Gid
            });
            projects.Add(projectDto);
        }

        return projects;
    }
    
    #endregion
}