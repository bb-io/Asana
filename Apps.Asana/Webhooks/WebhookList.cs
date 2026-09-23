using Apps.Asana.Actions;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Goals;
using Apps.Asana.Models.ProjectMemberships.Responses;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Sections.Requests;
using Apps.Asana.Models.Stories.Response;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Asana.Models.TeamMemberships;
using Apps.Asana.Models.Teams.Responses;
using Apps.Asana.Models.WorkspaceMemberships.Responses;
using Apps.Asana.Models.Workspaces.Requests;
using Apps.Asana.Webhooks.Handlers.GoalHandlers;
using Apps.Asana.Webhooks.Handlers.ProjectHandlers;
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
using Apps.Asana.Webhooks.Models.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using Apps.Asana.Api.Exceptions;
using Apps.Asana.Extensions;
using Apps.Asana.Webhooks.Models.Responses;

namespace Apps.Asana.Webhooks;

[WebhookList("Webhooks")]
public class WebhookList(InvocationContext invocationContext) 
    : BaseInvocable(invocationContext), IWebhookHandshakeHandler, IAsyncWebhookHandler
{
    public Task<HttpResponseMessage?> HandleHandshakeAsync(WebhookRequest request)
    {
        const string secretHeaderKey = "X-Hook-Secret";
        
        if (!request.TryGetHookSecret(secretHeaderKey, out var secretKey))
            return Task.FromResult<HttpResponseMessage?>(null);
        
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(string.Empty)
        };
        response.Headers.Add(secretHeaderKey, secretKey);
        
        return Task.FromResult<HttpResponseMessage?>(response);
    }
    
    private static async Task<WebhookResponse<List<TDto>>> HandleWebhookRequest<TDto>(
        WebhookRequest webhookRequest, 
        string action, 
        Func<Payload, Task<List<TDto>>> getEntitiesFromPayload)
    {
        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        if (payload == null || payload.Events == null || !payload.Events.Any())
            return CreatePreflightResponse<List<TDto>>();

        payload.Events = payload.Events.Where(x => x.Action == action).ToList();
        var entities = await getEntitiesFromPayload(payload);

        if (entities.Count == 0) 
            return CreatePreflightResponse<List<TDto>>();
        
        return new WebhookResponse<List<TDto>>
        {
            HttpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
            Result = entities,
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    private static WebhookResponse<List<DeletedItemResponse>> HandleDeletionWebhookRequest(
        WebhookRequest webhookRequest, 
        string action)
    {
        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        if (payload == null || payload.Events == null || !payload.Events.Any())
            return CreatePreflightResponse<List<DeletedItemResponse>>();

        var deletedIds = payload.Events
            .Where(x => x.Action == action && x.Resource?.Gid != null)
            .Select(x => x.Resource.Gid)
            .ToList();
        
        if (deletedIds.Count == 0)
            return CreatePreflightResponse<List<DeletedItemResponse>>();

        return new WebhookResponse<List<DeletedItemResponse>>
        {
            HttpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
            Result = deletedIds.Select(x => new DeletedItemResponse { ItemId = x }).ToList(),
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }
    
    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload, SectionRequest? sectionFilter)
    {
        string? sectionId = sectionFilter?.SectionId;

        if (!string.IsNullOrWhiteSpace(sectionId))
        {
            payload.Events = payload.Events!
                .Where(e => e.Parent?.ResourceType == "section" && e.Parent.Gid == sectionId)
                .ToList();
        }

        return await GetTasksFromPayload(payload);
    }
        
    #region Projects

    [MultipleEvents, Webhook("On projects added", typeof(ProjectsAddedHandler), Description = "Triggered when projects are added")]
    public Task<WebhookResponse<List<ProjectDto>>> ProjectsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetProjectsFromPayload);
    }

    [MultipleEvents, Webhook("On projects changed", typeof(ProjectChangedHandler), Description = "Triggered when projects are changed")]
    public Task<WebhookResponse<List<ProjectDto>>> ProjectChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetProjectsFromPayload);
    }

    [MultipleEvents, Webhook("On projects deleted", typeof(ProjectDeletedHandler), Description = "Triggered when projects are deleted")]
    public Task<WebhookResponse<List<DeletedItemResponse>>> ProjectsDeletedHandler(WebhookRequest webhookRequest)
    {
        return Task.FromResult(HandleDeletionWebhookRequest(webhookRequest, "deleted"));
    }

    [MultipleEvents, Webhook("On projects removed", typeof(ProjectRemovedHandler), Description = "Triggered when projects are removed")]
    public Task<WebhookResponse<List<ProjectDto>>> ProjectsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetProjectsFromPayload);
    }

    [MultipleEvents, Webhook("On projects undeleted", typeof(ProjectUndeletedHandler),
         Description = "Triggered when projects are undeleted")]
    public Task<WebhookResponse<List<ProjectDto>>> ProjectsUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetProjectsFromPayload);
    }

    #endregion

    #region Tasks

    [MultipleEvents, Webhook("On tasks added", typeof(TaskAddedHandler), Description = "Triggered when tasks are added")]
    public async Task<WebhookResponse<List<TaskDto>>> TasksAddedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] SectionRequest? sectionFilter,
        [WebhookParameter] SubtaskFilterRequest? subtaskFilter)
    {
        try
        {
            return await HandleWebhookRequest(webhookRequest, "added",
                async payload => SubtaskFilter.Apply(await GetTasksFromPayload(payload, sectionFilter),
                    subtaskFilter?.ExcludeSubtasks));
        }
        catch (AsanaResourceNotFoundException)
        {
            return CreatePreflightResponse<List<TaskDto>>();
        }
    }

    [MultipleEvents, Webhook("On tasks changed", typeof(TaskChangedHandler), Description = "Triggered when tasks are changed")]
    public Task<WebhookResponse<List<TaskDto>>> TasksChangedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] TaskCustomFieldsRequest fieldsRequest,
        [WebhookParameter] SubtaskFilterRequest? subtaskFilter)
    {
        return HandleWebhookRequest(webhookRequest, "changed",
            async payload => SubtaskFilter.Apply(await GetTasksFromPayload(payload, fieldsRequest),
                subtaskFilter?.ExcludeSubtasks));
    }

    [MultipleEvents, Webhook("On tasks deleted", typeof(TaskDeletedHandler), Description = "Triggered when tasks are deleted")]
    public Task<WebhookResponse<List<DeletedItemResponse>>> TasksDeletedHandler(WebhookRequest webhookRequest)
    {
        return Task.FromResult(HandleDeletionWebhookRequest(webhookRequest, "deleted"));
    }

    [MultipleEvents, Webhook("On tasks removed", typeof(TaskRemovedHandler), Description = "Triggered when tasks are removed")]
    public Task<WebhookResponse<List<TaskDto>>> TasksRemovedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] SubtaskFilterRequest? subtaskFilter)
    {
        return HandleWebhookRequest(webhookRequest, "removed",
            async payload => SubtaskFilter.Apply(await GetTasksFromPayload(payload), subtaskFilter?.ExcludeSubtasks));
    }

    [MultipleEvents, Webhook("On tasks undeleted", typeof(TaskUndeletedHandler), Description = "Triggered when tasks are undeleted")]
    public Task<WebhookResponse<List<TaskDto>>> TasksUndeletedHandler(WebhookRequest webhookRequest,
        [WebhookParameter] SubtaskFilterRequest? subtaskFilter)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted",
            async payload => SubtaskFilter.Apply(await GetTasksFromPayload(payload), subtaskFilter?.ExcludeSubtasks));
    }

    #endregion

    #region Tags

    [MultipleEvents, Webhook("On tags added", typeof(TagAddedHandler), Description = "Triggered when tags are added")]
    public Task<WebhookResponse<List<TagDto>>> TagsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetTagsFromPayload);
    }

    [MultipleEvents, Webhook("On tags changed", typeof(TagChangedHandler), Description = "Triggered when tags are changed")]
    public Task<WebhookResponse<List<TagDto>>> TagsChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetTagsFromPayload);
    }

    [MultipleEvents, Webhook("On tags deleted", typeof(TagDeletedHandler), Description = "Triggered when tags are deleted")]
    public Task<WebhookResponse<List<DeletedItemResponse>>> TagsDeletedHandler(WebhookRequest webhookRequest)
    {
        return Task.FromResult(HandleDeletionWebhookRequest(webhookRequest, "deleted"));
    }
    
    [MultipleEvents, Webhook("On tags undeleted", typeof(TagUndeletedHandler), Description = "Triggered when tags are undeleted")]
    public Task<WebhookResponse<List<TagDto>>> TagsUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetTagsFromPayload);
    }
    
    #endregion

    #region Sections

    [MultipleEvents, Webhook("On sections added", typeof(SectionAddedHandler), Description = "Triggered when sections are added")]
    public Task<WebhookResponse<List<AsanaEntity>>> SectionsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetSectionsFromPayload);
    }

    [MultipleEvents, Webhook("On sections changed", typeof(SectionChangedHandler), Description = "Triggered when sections are changed")]
    public Task<WebhookResponse<List<AsanaEntity>>> SectionsChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetSectionsFromPayload);
    }

    [MultipleEvents, Webhook("On sections deleted", typeof(SectionDeletedHandler), Description = "Triggered when sections are deleted")]
    public WebhookResponse<List<DeletedItemResponse>> SectionsDeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleDeletionWebhookRequest(webhookRequest, "deleted");
    }

    [MultipleEvents, Webhook("On sections undeleted", typeof(SectionUndeletedHandler), Description = "Triggered when sections are undeleted")]
    public Task<WebhookResponse<List<AsanaEntity>>> SectionsUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetSectionsFromPayload);
    }

    #endregion

    #region Stories

    [MultipleEvents, Webhook("On stories added", typeof(StoriesAddedHandler), Description = "Triggered when stories are added")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetStoriesFromPayload);
    }

    [MultipleEvents, Webhook("On stories removed", typeof(StoriesRemovedHandler), Description = "Triggered when stories are removed")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetStoriesFromPayload);
    }

    [MultipleEvents, Webhook("On stories undeleted", typeof(StoriesUndeletedHandler), Description = "Triggered when stories are undeleted")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetStoriesFromPayload);
    }

    #endregion

    #region StoriesComments
    
    [MultipleEvents, Webhook("On stories comments added", typeof(StoriesCommentsAddedHandler),
         Description = "Triggered when comments are added to stories")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesCommentsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetStoriesFromPayload);
    }

    [MultipleEvents, Webhook("On stories comments changed", typeof(StoriesCommentsChangedHandler),
         Description = "Triggered when comments on stories are changed")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesCommentsChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetStoriesFromPayload);
    }

    [MultipleEvents, Webhook("On stories comments removed", typeof(StoriesCommentsRemovedHandler),
         Description = "Triggered when comments are removed from stories")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesCommentsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetStoriesFromPayload);
    }

    [MultipleEvents, Webhook("On stories comments undeleted", typeof(StoriesCommentsUndeletedHandler),
         Description = "Triggered when comments on stories are undeleted")]
    public Task<WebhookResponse<List<StoryResponse>>> StoriesCommentsUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetStoriesFromPayload);
    }

    #endregion

    #region Workspaces

    [MultipleEvents, Webhook("On workspaces changed", typeof(WorkspaceChangedHandler),
         Description = "Triggered when changes are made to workspaces")]
    public Task<WebhookResponse<List<WorkspaceDto>>> WorkspaceChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetWorkspacesFromPayload);
    }
    
    #endregion

    #region Goals

    [MultipleEvents, Webhook("On goals added", typeof(GoalsAddedHandler), Description = "Triggered when goals are added")]
    public Task<WebhookResponse<List<GoalResponse>>> GoalsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetGoalsFromPayload);
    }

    [MultipleEvents, Webhook("On goals changed", typeof(GoalsChangedHandler), Description = "Triggered when goals are changed")]
    public Task<WebhookResponse<List<GoalResponse>>> GoalsChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetGoalsFromPayload);
    }

    [MultipleEvents, Webhook("On goals removed", typeof(GoalsRemovedHandler), Description = "Triggered when goals are removed")]
    public Task<WebhookResponse<List<GoalResponse>>> GoalsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetGoalsFromPayload);
    }

    [MultipleEvents, Webhook("On goals deleted", typeof(GoalsDeletedHandler), Description = "Triggered when goals are deleted")]
    public WebhookResponse<List<DeletedItemResponse>> GoalsDeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleDeletionWebhookRequest(webhookRequest, "deleted");
    }

    [MultipleEvents, Webhook("On goals undeleted", typeof(GoalsUndeletedHandler), Description = "Triggered when goals are undeleted")]
    public Task<WebhookResponse<List<GoalResponse>>> GoalsUndeletedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "undeleted", GetGoalsFromPayload);
    }

    #endregion

    #region ProjectMemberships

    [MultipleEvents, Webhook("On project memberships added", typeof(ProjectMembershipsAddedHandler),
         Description = "Triggered when project memberships are added")]
    public Task<WebhookResponse<List<ProjectMembershipResponse>>> ProjectMembershipsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetProjectMembershipsFromPayload);
    }

    [MultipleEvents, Webhook("On project memberships removed", typeof(ProjectMembershipsRemovedHandler),
         Description = "Triggered when project memberships are removed")]
    public Task<WebhookResponse<List<ProjectMembershipResponse>>> ProjectMembershipsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetProjectMembershipsFromPayload);
    }

    #endregion

    #region Teams

    [MultipleEvents, Webhook("On teams added", typeof(TeamsAddedHandler), Description = "Triggered when teams are added")]
    public Task<WebhookResponse<List<TeamResponse>>> TeamsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetTeamsFromPayload);
    }

    [MultipleEvents, Webhook("On teams changed", typeof(TeamsChangedHandler), Description = "Triggered when teams are changed")]
    public Task<WebhookResponse<List<TeamResponse>>> TeamsChangedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "changed", GetTeamsFromPayload);
    }

    [MultipleEvents, Webhook("On teams deleted", typeof(TeamsDeletedHandler), Description = "Triggered when teams are deleted")]
    public Task<WebhookResponse<List<DeletedItemResponse>>> TeamsDeletedHandler(WebhookRequest webhookRequest)
    {
        return Task.FromResult(HandleDeletionWebhookRequest(webhookRequest, "deleted"));
    }

    #endregion

    #region TeamMemberships

    [MultipleEvents, Webhook("On team memberships added", typeof(TeamMembershipsAddedHandler), 
         Description = "Triggered when team memberships are added")]
    public Task<WebhookResponse<List<TeamMembershipResponse>>> TeamMembershipsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetTeamMembershipsFromPayload);
    }

    [MultipleEvents, Webhook("On team memberships removed", typeof(TeamMembershipsRemovedHandler),
         Description = "Triggered when team memberships are removed")]
    public Task<WebhookResponse<List<TeamMembershipResponse>>> TeamMembershipsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetTeamMembershipsFromPayload);
    }

    #endregion

    #region WorkspaceMemberships

    [MultipleEvents, Webhook("On workspace memberships added", typeof(WorkspaceMembershipsAddedHandler),
         Description = "Triggered when workspace memberships are added")]
    public Task<WebhookResponse<List<WorkspaceMembershipResponse>>> WorkspaceMembershipsAddedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "added", GetWorkspaceMembershipsFromPayload);
    }

    [MultipleEvents, Webhook("On workspace memberships removed", typeof(WorkspaceMembershipsRemovedHandler),
         Description = "Triggered when workspace memberships are removed")]
    public Task<WebhookResponse<List<WorkspaceMembershipResponse>>> WorkspaceMembershipsRemovedHandler(WebhookRequest webhookRequest)
    {
        return HandleWebhookRequest(webhookRequest, "removed", GetWorkspaceMembershipsFromPayload);
    }

    #endregion

    #region Utils

    private static WebhookResponse<T> CreatePreflightResponse<T>() where T : class
    {
        return new WebhookResponse<T>
        {
            HttpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
            Result = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
    }

    private async Task<List<TDto>> GetEntitiesFromPayload<TDto, TRequest, TAction>(
        Payload payload, Func<InvocationContext, TAction> createAction, Func<Event, TRequest> createRequest,
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

    private async Task<List<ProjectDto>> GetProjectsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new ProjectActions(context),
            item => new ProjectRequest { ProjectId = item.Resource.Gid },
            (action, request) => action.GetProject(request));

    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new TaskActions(context),
            item => new TaskRequest { TaskId = item.Resource.Gid }, (action, request) => action.GetTask(request));
    
    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload, TaskCustomFieldsRequest request)
    {
        if (request.CustomFieldIds != null && request.CustomFieldIds.Any())
        {
            payload.Events = payload.Events?.Where(e =>
            {
                if (e.Change?.Field != "custom_fields")
                    return false;

                var changeJson = JsonConvert.SerializeObject(e.Change);
                return request.CustomFieldIds.Any(targetId => changeJson.Contains(targetId));
            }).ToList();
        }

        if (payload.Events == null || payload.Events.Count == 0)
            return [];

        var tasks = await GetEntitiesFromPayload(
            payload,
            context => new TaskActions(context),
            item => new TaskRequest { TaskId = item.Resource.Gid },
            (action, req) => action.GetTask(req));

        return tasks;
    }

    private async Task<List<TagDto>> GetTagsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new TagActions(context),
            item => new TagRequest { TagId = item.Resource.Gid }, (action, request) => action.GetTag(request));

    private async Task<List<AsanaEntity>> GetSectionsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new SectionActions(context),
            item => new SectionRequest { SectionId = item.Resource.Gid },
            (action, request) => action.GetSection(request));

    private async Task<List<WorkspaceDto>> GetWorkspacesFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new WorkspaceActions(context),
            item => new WorkspaceRequest { WorkspaceId = item.Resource.Gid },
            (action, request) => action.GetWorkspace(request));

    private async Task<List<GoalResponse>> GetGoalsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Goals}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<GoalDto>(request)
                .ContinueWith(task => new GoalResponse(task.Result)));

    private async Task<List<ProjectMembershipResponse>> GetProjectMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.ProjectMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<ProjectMembershipDto>(request)
                .ContinueWith(task => new ProjectMembershipResponse(task.Result)));

    private async Task<List<StoryResponse>> GetStoriesFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Stories}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<StoryDto>(request)
                .ContinueWith(task => new StoryResponse(task.Result)));

    private async Task<List<TeamResponse>> GetTeamsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.Teams}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<TeamDto>(request)
                .ContinueWith(task => new TeamResponse(task.Result)));

    private async Task<List<TeamMembershipResponse>> GetTeamMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.TeamMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<TeamMembershipDto>(request)
                .ContinueWith(task => new TeamMembershipResponse(task.Result)));

    private async Task<List<WorkspaceMembershipResponse>> GetWorkspaceMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(),
            item => new AsanaRequest($"{ApiEndpoints.WorkspaceMemberships}/{item.Resource.Gid}", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders),
            (client, request) => client.ExecuteWithErrorHandling<WorkspaceMembershipDto>(request)
                .ContinueWith(task => new WorkspaceMembershipResponse(task.Result)));

    #endregion
}
