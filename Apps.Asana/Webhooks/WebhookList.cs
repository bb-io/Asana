using Apps.Asana.Dtos;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;
using Apps.Asana.Actions;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Goals;
using Apps.Asana.Models.ProjectMemberships.Responses;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Stories.Response;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.TeamMemberships;
using Apps.Asana.Models.Teams.Responses;
using Apps.Asana.Models.WorkspaceMemberships.Responses;
using Apps.Asana.Models.Workspaces.Requests;
using Apps.Asana.Webhooks.Handlers.GoalHandlers;
using Apps.Asana.Webhooks.Handlers.ProjectMemberships;
using Apps.Asana.Webhooks.Handlers.SectionHandlers;
using Apps.Asana.Webhooks.Handlers.StoryCommentHandlers;
using Apps.Asana.Webhooks.Handlers.StoryHandlers;
using Apps.Asana.Webhooks.Handlers.TagHandlers;
using Apps.Asana.Webhooks.Handlers.TaskHandlers;
using Apps.Asana.Webhooks.Handlers.TeamHandlers;
using Apps.Asana.Webhooks.Handlers.TeamMembershipHandlers;
using Apps.Asana.Webhooks.Handlers.WorkspaceHandlers;
using Apps.Asana.Webhooks.Handlers.WorkspaceMembershipHandlers;
using Apps.Asana.Webhooks.Models;
using Apps.Asana.Webhooks.Models.Payload;
using Apps.Asana.Webhooks.Models.Responses.Projects;
using Apps.Asana.Webhooks.Models.Responses.Sections;
using Apps.Asana.Webhooks.Models.Responses.Tags;
using Apps.Asana.Webhooks.Models.Responses.Tasks;
using Apps.Asana.Webhooks.Models.Responses.Workspaces;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

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

    #region Stories
    
    [Webhook("On stories added", typeof(StoriesAddedHandler),
        Description = "Triggered when stories are added")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var stories = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = stories },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On stories removed", typeof(StoriesRemovedHandler),
        Description = "Triggered when stories are removed")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesRemovedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var stories = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = stories },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On stories undeleted", typeof(StoriesUndeletedHandler),
        Description = "Triggered when stories are undeleted")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var stories = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = stories },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region StoriesComments

    [Webhook("On stories comments added", typeof(StoriesCommentsAddedHandler),
        Description = "Triggered when comments are added to stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsAddedHandler(
        WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var storiesComments = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = storiesComments },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On stories comments changed", typeof(StoriesCommentsChangedHandler),
        Description = "Triggered when changes are made to comments on stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsChangedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var storiesComments = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = storiesComments },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On stories comments removed", typeof(StoriesCommentsRemovedHandler),
        Description = "Triggered when comments are removed from stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsRemovedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var storiesComments = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = storiesComments },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On stories comments undeleted", typeof(StoriesCommentsUndeletedHandler),
        Description = "Triggered when comments are undeleted from stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsUndeletedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<StoriesResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<StoriesResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var storiesComments = await GetStoriesFromPayload(payload);
        return new WebhookResponse<StoriesResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new StoriesResponse { Stories = storiesComments },
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

    #region Goals

    [Webhook("On goals added", typeof(GoalsAddedHandler),
        Description = "Triggered when goals are added")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<GoalsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<GoalsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var goals = await GetGoalsFromPayload(payload);
        return new WebhookResponse<GoalsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new GoalsResponse { Goals = goals },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On goals changed", typeof(GoalsChangedHandler),
        Description = "Triggered when changes are made to goals")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<GoalsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<GoalsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var goals = await GetGoalsFromPayload(payload);
        return new WebhookResponse<GoalsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new GoalsResponse { Goals = goals },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On goals removed", typeof(GoalsRemovedHandler),
        Description = "Triggered when goals are removed")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsRemovedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<GoalsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<GoalsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var goals = await GetGoalsFromPayload(payload);
        return new WebhookResponse<GoalsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new GoalsResponse { Goals = goals },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On goals deleted", typeof(GoalsDeletedHandler),
        Description = "Triggered when goals are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> GoalsDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<DeletedItemsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<DeletedItemsResponse>();
        }

        return new WebhookResponse<DeletedItemsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new DeletedItemsResponse
            {
                ItemIds = payload.Events
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On goals undeleted", typeof(GoalsUndeletedHandler),
        Description = "Triggered when goals are undeleted")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsUndeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<GoalsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<GoalsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "undeleted").ToList();
        var goals = await GetGoalsFromPayload(payload);
        return new WebhookResponse<GoalsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new GoalsResponse { Goals = goals },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion
    
    #region ProjectMemberships

    [Webhook("On project memberships added", typeof(ProjectMembershipsAddedHandler),
        Description = "Triggered when project memberships are added")]
    public async Task<WebhookResponse<ProjectMembershipsResponse>> ProjectMembershipsAddedHandler(
        WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var projectMemberships = await GetProjectMembershipsFromPayload(payload);
        return new WebhookResponse<ProjectMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new ProjectMembershipsResponse { ProjectMemberships = projectMemberships },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On project memberships removed", typeof(ProjectMembershipsRemovedHandler),
        Description = "Triggered when project memberships are removed")]
    public async Task<WebhookResponse<ProjectMembershipsResponse>> ProjectMembershipsRemovedHandler(
        WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<ProjectMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<ProjectMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var projectMemberships = await GetProjectMembershipsFromPayload(payload);
        return new WebhookResponse<ProjectMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new ProjectMembershipsResponse { ProjectMemberships = projectMemberships },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region Teams

    [Webhook("On teams added", typeof(TeamsAddedHandler),
        Description = "Triggered when teams are added")]
    public async Task<WebhookResponse<TeamsResponse>> TeamsAddedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TeamsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TeamsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var teams = await GetTeamsFromPayload(payload);
        return new WebhookResponse<TeamsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TeamsResponse { Teams = teams },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On teams changed", typeof(TeamsChangedHandler),
        Description = "Triggered when changes are made to teams")]
    public async Task<WebhookResponse<TeamsResponse>> TeamsChangedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TeamsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TeamsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "changed").ToList();
        var teams = await GetTeamsFromPayload(payload);
        return new WebhookResponse<TeamsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TeamsResponse { Teams = teams },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On teams deleted", typeof(TeamsDeletedHandler),
        Description = "Triggered when teams are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> TeamsDeletedHandler(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<DeletedItemsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<DeletedItemsResponse>();
        }

        return new WebhookResponse<DeletedItemsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new DeletedItemsResponse
            {
                ItemIds = payload.Events
                    .Where(x => x.Action == "deleted" && x.Resource?.Gid != null)
                    .Select(x => x.Resource.Gid)
                    .ToList()
            },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }
    
    #endregion

    #region TeamMemberships

    [Webhook("On team memberships added", typeof(TeamMembershipsAddedHandler),
        Description = "Triggered when team memberships are added")]
    public async Task<WebhookResponse<TeamMembershipsResponse>> TeamMembershipsAddedHandler(
        WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TeamMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TeamMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var teamMemberships = await GetTeamMembershipsFromPayload(payload);
        return new WebhookResponse<TeamMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TeamMembershipsResponse { TeamMemberships = teamMemberships },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On team memberships removed", typeof(TeamMembershipsRemovedHandler),
        Description = "Triggered when team memberships are removed")]
    public async Task<WebhookResponse<TeamMembershipsResponse>> TeamMembershipsRemovedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TeamMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<TeamMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var teamMemberships = await GetTeamMembershipsFromPayload(payload);
        return new WebhookResponse<TeamMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new TeamMembershipsResponse { TeamMemberships = teamMemberships },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #endregion

    #region WorkspaceMemberships

    [Webhook("On workspace memberships added", typeof(WorkspaceMembershipsAddedHandler),
        Description = "Triggered when workspace memberships are added")]
    public async Task<WebhookResponse<WorkspaceMembershipsResponse>> WorkspaceMembershipsAddedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<WorkspaceMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<WorkspaceMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "added").ToList();
        var workspaceMemberships = await GetWorkspaceMembershipsFromPayload(payload);
        return new WebhookResponse<WorkspaceMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new WorkspaceMembershipsResponse { WorkspaceMemberships = workspaceMemberships },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    [Webhook("On workspace memberships removed", typeof(WorkspaceMembershipsRemovedHandler),
        Description = "Triggered when workspace memberships are removed")]
    public async Task<WebhookResponse<WorkspaceMembershipsResponse>> WorkspaceMembershipsRemovedHandler(
        WebhookRequest request)
    {
        if (request.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<WorkspaceMembershipsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(request.Body.ToString()!);
        await Logger.LogAsync(new
        {
            Events = payload
        });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<WorkspaceMembershipsResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == "removed").ToList();
        var workspaceMemberships = await GetWorkspaceMembershipsFromPayload(payload);
        return new WebhookResponse<WorkspaceMembershipsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            },
            Result = new WorkspaceMembershipsResponse { WorkspaceMemberships = workspaceMemberships },
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

    private async Task<List<TDto>> GetEntitiesFromPayload<TDto, TRequest, TAction>(Payload payload,
        Func<InvocationContext, TAction> createAction, Func<Event, TRequest> createRequest,
        Func<TAction, TRequest, Task<TDto>> fetchEntity)
    {
        var action = createAction(InvocationContext);
        var entities = new List<TDto>();

        foreach (var item in payload.Events!.Where(x => x.Resource?.Gid != null).DistinctBy(x => x.Resource.Gid))
        {
            var request = createRequest(item);
            var dto = await fetchEntity(action, request);
            entities.Add(dto);
        }

        return entities;
    }

    private async Task<List<ProjectDto>> GetProjectsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new ProjectActions(context),
            item => new ProjectRequest { ProjectId = item.Resource.Gid },
            (action, request) => action.GetProject(request));
    }

    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new TaskActions(context),
            item => new TaskRequest { TaskId = item.Resource.Gid },
            (action, request) => action.GetTask(request));
    }

    private async Task<List<TagDto>> GetTagsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new TagActions(context),
            item => new TagRequest { TagId = item.Resource.Gid },
            (action, request) => action.GetTag(request));
    }

    private async Task<List<AsanaEntity>> GetSectionsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new SectionActions(context),
            item => new SectionRequest { SectionId = item.Resource.Gid },
            (action, request) => action.GetSection(request));
    }

    private async Task<List<WorkspaceDto>> GetWorkspacesFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new WorkspaceActions(context),
            item => new WorkspaceRequest { WorkspaceId = item.Resource.Gid },
            (action, request) => action.GetWorkspace(request));
    }

    private async Task<List<GoalResponse>> GetGoalsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Goals}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<GoalDto>(request)
                .ContinueWith(task => new GoalResponse(task.Result)));
    }

    private async Task<List<ProjectMembershipResponse>> GetProjectMembershipsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.ProjectMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<ProjectMembershipDto>(request)
                .ContinueWith(task => new ProjectMembershipResponse(task.Result)));
    }

    private async Task<List<StoryResponse>> GetStoriesFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Stories}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<StoryDto>(request)
                .ContinueWith(task => new StoryResponse(task.Result)));
    }

    private async Task<List<TeamResponse>> GetTeamsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Teams}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<TeamDto>(request)
                .ContinueWith(task => new TeamResponse(task.Result)));
    }

    private async Task<List<TeamMembershipResponse>> GetTeamMembershipsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.TeamMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<TeamMembershipDto>(request)
                .ContinueWith(task => new TeamMembershipResponse(task.Result)));
    }

    private async Task<List<WorkspaceMembershipResponse>> GetWorkspaceMembershipsFromPayload(Payload payload)
    {
        return await GetEntitiesFromPayload(payload,
            context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.WorkspaceMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<WorkspaceMembershipDto>(request)
                .ContinueWith(task => new WorkspaceMembershipResponse(task.Result)));
    }

    #endregion
}