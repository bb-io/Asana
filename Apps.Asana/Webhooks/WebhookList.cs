using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Actions;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Webhooks.Handlers.SectionHandlers;
using Apps.Asana.Webhooks.Handlers.TagHandlers;
using Apps.Asana.Webhooks.Handlers.TaskHandlers;
using Apps.Asana.Webhooks.Handlers.WorkspaceHandlers;
using Apps.Asana.Webhooks.Models.Payload;
using Apps.Asana.Webhooks.Models.Responses.Projects;
using Apps.Asana.Webhooks.Models.Responses.Sections;
using Apps.Asana.Webhooks.Models.Responses.Tags;
using Apps.Asana.Webhooks.Models.Responses.Tasks;
using Apps.Asana.Webhooks.Models.Responses.Workspaces;
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
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
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

    #region Tasks

    [Webhook("On tasks added", typeof(TaskAddedHandler),
        Description = "Triggered when tasks are added")]
    public async Task<WebhookResponse<TasksResponse>> TasksAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TasksResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TasksResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var tasks = await GetTasksFromPayload(payload);
        return new WebhookResponse<TasksResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TasksResponse { Tasks = tasks },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tasks changed", typeof(TaskChangedHandler),
        Description = "Triggered when changes are made to tasks")]
    public async Task<WebhookResponse<TasksResponse>> TasksChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TasksResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TasksResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var tasks = await GetTasksFromPayload(payload);
        return new WebhookResponse<TasksResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TasksResponse { Tasks = tasks },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tasks deleted", typeof(TaskDeletedHandler),
        Description = "Triggered when tasks are deleted")]
    public async Task<WebhookResponse<TasksDeletedResponse>> TasksDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TasksDeletedResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TasksDeletedResponse>();
        }

        return new WebhookResponse<TasksDeletedResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TasksDeletedResponse
            {
                DeletedTaskIds = payload.Events
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tasks removed", typeof(TaskRemovedHandler),
        Description = "Triggered when tasks are removed")]
    public async Task<WebhookResponse<TasksResponse>> TasksRemovedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TasksResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TasksResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var tasks = await GetTasksFromPayload(payload);
        return new WebhookResponse<TasksResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TasksResponse { Tasks = tasks },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tasks undeleted", typeof(TaskUndeletedHandler),
        Description = "Triggered when tasks are undeleted")]
    public async Task<WebhookResponse<TasksResponse>> TasksUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TasksResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TasksResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var tasks = await GetTasksFromPayload(payload);
        return new WebhookResponse<TasksResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TasksResponse { Tasks = tasks },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region Tags

    [Webhook("On tags added", typeof(TagAddedHandler),
        Description = "Triggered when tags are added")]
    public async Task<WebhookResponse<TagsResponse>> TagsAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TagsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TagsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var tags = await GetTagsFromPayload(payload);
        return new WebhookResponse<TagsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TagsResponse { Tags = tags },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tags changed", typeof(TagChangedHandler),
        Description = "Triggered when changes are made to tags")]
    public async Task<WebhookResponse<TagsResponse>> TagsChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TagsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TagsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var tags = await GetTagsFromPayload(payload);
        return new WebhookResponse<TagsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TagsResponse { Tags = tags },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tags deleted", typeof(TagDeletedHandler),
        Description = "Triggered when tags are deleted")]
    public async Task<WebhookResponse<TagsDeletedResponse>> TagsDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TagsDeletedResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TagsDeletedResponse>();
        }

        return new WebhookResponse<TagsDeletedResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TagsDeletedResponse
            {
                DeletedTagIds = payload.Events
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On tags undeleted", typeof(TagUndeletedHandler),
        Description = "Triggered when tags are undeleted")]
    public async Task<WebhookResponse<TagsResponse>> TagsUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TagsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TagsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var tags = await GetTagsFromPayload(payload);
        return new WebhookResponse<TagsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TagsResponse { Tags = tags },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region Sections

    [Webhook("On sections added", typeof(SectionAddedHandler),
        Description = "Triggered when sections are added")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<SectionsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<SectionsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var sections = await GetSectionsFromPayload(payload);
        return new WebhookResponse<SectionsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new SectionsResponse { Sections = sections },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On sections changed", typeof(SectionChangedHandler),
        Description = "Triggered when changes are made to sections")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<SectionsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<SectionsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var sections = await GetSectionsFromPayload(payload);
        return new WebhookResponse<SectionsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new SectionsResponse { Sections = sections },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On sections deleted", typeof(SectionDeletedHandler),
        Description = "Triggered when sections are deleted")]
    public async Task<WebhookResponse<SectionsDeletedResponse>> SectionsDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<SectionsDeletedResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<SectionsDeletedResponse>();
        }

        return new WebhookResponse<SectionsDeletedResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new SectionsDeletedResponse
            {
                DeletedSectionIds = payload.Events
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On sections undeleted", typeof(SectionUndeletedHandler),
        Description = "Triggered when sections are undeleted")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<SectionsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<SectionsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var sections = await GetSectionsFromPayload(payload);
        return new WebhookResponse<SectionsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new SectionsResponse { Sections = sections },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region Workspaces

    [Webhook("On workspaces changed", typeof(WorkspaceChangedHandler),
        Description = "Triggered when changes are made to workspaces")]
    public async Task<WebhookResponse<WorkspacesResponse>> WorkspaceChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<WorkspacesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<WorkspacesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var workspaces = await GetWorkspacesFromPayload(payload);
        return new WebhookResponse<WorkspacesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new WorkspacesResponse { Workspaces = workspaces },
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

    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload)
    {
        var taskActions = new TaskActions(InvocationContext);
        var tasks = new List<TaskDto>();

        foreach (var task in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var taskDto = await taskActions.GetTask(new TaskRequest
            {
                TaskId = task.Resource.Gid
            });
            tasks.Add(taskDto);
        }

        return tasks;
    }

    private async Task<List<TagDto>> GetTagsFromPayload(Payload payload)
    {
        var tagActions = new TagActions(InvocationContext);
        var tags = new List<TagDto>();

        foreach (var tag in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var tagDto = await tagActions.GetTag(new TagRequest
            {
                TagId = tag.Resource.Gid
            });
            tags.Add(tagDto);
        }

        return tags;
    }

    private async Task<List<AsanaEntity>> GetSectionsFromPayload(Payload payload)
    {
        var sectionActions = new SectionActions(InvocationContext);
        var sections = new List<AsanaEntity>();

        foreach (var section in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var sectionDto = await sectionActions.GetSection(new SectionRequest
            {
                SectionId = section.Resource.Gid
            });
            sections.Add(sectionDto);
        }

        return sections;
    }
    
    private async Task<List<WorkspaceDto>> GetWorkspacesFromPayload(Payload payload)
    {
        var sectionActions = new WorkspaceActions(InvocationContext);
        var workspaces = new List<WorkspaceDto>();

        foreach (var section in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var sectionDto = await sectionActions.GetWorkspace(new SectionRequest
            {
                SectionId = section.Resource.Gid
            });
            workspaces.Add(sectionDto);
        }

        return workspaces;
    }

    #endregion
}