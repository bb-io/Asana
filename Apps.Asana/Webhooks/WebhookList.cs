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

    private async Task<WebhookResponse<TResponse>> HandleWebhookRequest<TResponse, TDto>(
        WebhookRequest webhookRequest, string action, Func<Payload, Task<List<TDto>>> getEntitiesFromPayload)
        where TResponse : class, new()
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<TResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new { Events = payload });

        if (payload == null || payload.Events == null || !payload.Events.Any())
        {
            return CreatePreflightResponse<TResponse>();
        }

        payload.Events = payload.Events.Where(x => x.Action == action).ToList();
        var entities = await getEntitiesFromPayload(payload);

        return new WebhookResponse<TResponse>
        {
            HttpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
            Result = (dynamic)new { Entities = entities },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    private async Task<WebhookResponse<DeletedItemsResponse>> HandleDeletionWebhookRequest<TResponse>(
        WebhookRequest webhookRequest, string action)
        where TResponse : DeletedItemsResponse, new()
    {
        if (webhookRequest.Headers.TryGetValue(SecretHeaderKey, out var secretKey))
        {
            return CreatePreflightResponse<DeletedItemsResponse>(secretKey);
        }

        var payload = JsonConvert.DeserializeObject<Payload>(webhookRequest.Body.ToString()!);
        await Logger.LogAsync(new { Events = payload });

        if (payload == null || payload.Events == null)
        {
            return CreatePreflightResponse<DeletedItemsResponse>();
        }

        var deletedIds = payload.Events
            .Where(x => x.Action == action && x.Resource?.Gid != null)
            .Select(x => x.Resource.Gid)
            .ToList();

        return new WebhookResponse<DeletedItemsResponse>
        {
            HttpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
            Result = new TResponse { ItemIds = deletedIds },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }

    #region Projects

    [Webhook("On projects added", typeof(ProjectsAddedHandler), Description = "Triggered when projects are added")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectsResponse, ProjectDto>(webhookRequest, "added", GetProjectsFromPayload);

    [Webhook("On projects changed", typeof(ProjectChangedHandler), Description = "Triggered when projects are changed")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectsResponse, ProjectDto>(webhookRequest, "changed", GetProjectsFromPayload);

    [Webhook("On projects deleted", typeof(ProjectDeletedHandler), Description = "Triggered when projects are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> ProjectsDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    [Webhook("On projects removed", typeof(ProjectRemovedHandler), Description = "Triggered when projects are removed")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectsResponse, ProjectDto>(webhookRequest, "removed", GetProjectsFromPayload);

    [Webhook("On projects undeleted", typeof(ProjectUndeletedHandler), Description = "Triggered when projects are undeleted")]
    public async Task<WebhookResponse<ProjectsResponse>> ProjectsUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectsResponse, ProjectDto>(webhookRequest, "undeleted", GetProjectsFromPayload);

    #endregion

    #region Tasks

    [Webhook("On tasks added", typeof(TaskAddedHandler), Description = "Triggered when tasks are added")]
    public async Task<WebhookResponse<TasksResponse>> TasksAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TasksResponse, TaskDto>(webhookRequest, "added", GetTasksFromPayload);

    [Webhook("On tasks changed", typeof(TaskChangedHandler), Description = "Triggered when tasks are changed")]
    public async Task<WebhookResponse<TasksResponse>> TasksChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TasksResponse, TaskDto>(webhookRequest, "changed", GetTasksFromPayload);

    [Webhook("On tasks deleted", typeof(TaskDeletedHandler), Description = "Triggered when tasks are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> TasksDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    [Webhook("On tasks removed", typeof(TaskRemovedHandler), Description = "Triggered when tasks are removed")]
    public async Task<WebhookResponse<TasksResponse>> TasksRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TasksResponse, TaskDto>(webhookRequest, "removed", GetTasksFromPayload);

    [Webhook("On tasks undeleted", typeof(TaskUndeletedHandler), Description = "Triggered when tasks are undeleted")]
    public async Task<WebhookResponse<TasksResponse>> TasksUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TasksResponse, TaskDto>(webhookRequest, "undeleted", GetTasksFromPayload);

    #endregion

    #region Tags

    [Webhook("On tags added", typeof(TagAddedHandler), Description = "Triggered when tags are added")]
    public async Task<WebhookResponse<TagsResponse>> TagsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TagsResponse, TagDto>(webhookRequest, "added", GetTagsFromPayload);

    [Webhook("On tags changed", typeof(TagChangedHandler), Description = "Triggered when tags are changed")]
    public async Task<WebhookResponse<TagsResponse>> TagsChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TagsResponse, TagDto>(webhookRequest, "changed", GetTagsFromPayload);

    [Webhook("On tags deleted", typeof(TagDeletedHandler), Description = "Triggered when tags are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> TagsDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    [Webhook("On tags undeleted", typeof(TagUndeletedHandler), Description = "Triggered when tags are undeleted")]
    public async Task<WebhookResponse<TagsResponse>> TagsUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TagsResponse, TagDto>(webhookRequest, "undeleted", GetTagsFromPayload);

    #endregion

    #region Sections

    [Webhook("On sections added", typeof(SectionAddedHandler), Description = "Triggered when sections are added")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<SectionsResponse, AsanaEntity>(webhookRequest, "added", GetSectionsFromPayload);

    [Webhook("On sections changed", typeof(SectionChangedHandler), Description = "Triggered when sections are changed")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<SectionsResponse, AsanaEntity>(webhookRequest, "changed", GetSectionsFromPayload);

    [Webhook("On sections deleted", typeof(SectionDeletedHandler), Description = "Triggered when sections are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> SectionsDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    [Webhook("On sections undeleted", typeof(SectionUndeletedHandler), Description = "Triggered when sections are undeleted")]
    public async Task<WebhookResponse<SectionsResponse>> SectionsUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<SectionsResponse, AsanaEntity>(webhookRequest, "undeleted", GetSectionsFromPayload);

    #endregion

    #region Stories

    [Webhook("On stories added", typeof(StoriesAddedHandler), Description = "Triggered when stories are added")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "added", GetStoriesFromPayload);

    [Webhook("On stories removed", typeof(StoriesRemovedHandler), Description = "Triggered when stories are removed")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "removed", GetStoriesFromPayload);

    [Webhook("On stories undeleted", typeof(StoriesUndeletedHandler), Description = "Triggered when stories are undeleted")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "undeleted", GetStoriesFromPayload);

    #endregion

    #region StoriesComments

    [Webhook("On stories comments added", typeof(StoriesCommentsAddedHandler), Description = "Triggered when comments are added to stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "added", GetStoriesFromPayload);

    [Webhook("On stories comments changed", typeof(StoriesCommentsChangedHandler), Description = "Triggered when comments on stories are changed")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "changed", GetStoriesFromPayload);

    [Webhook("On stories comments removed", typeof(StoriesCommentsRemovedHandler), Description = "Triggered when comments are removed from stories")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "removed", GetStoriesFromPayload);

    [Webhook("On stories comments undeleted", typeof(StoriesCommentsUndeletedHandler), Description = "Triggered when comments on stories are undeleted")]
    public async Task<WebhookResponse<StoriesResponse>> StoriesCommentsUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<StoriesResponse, StoryResponse>(webhookRequest, "undeleted", GetStoriesFromPayload);

    #endregion

    #region Workspaces

    [Webhook("On workspaces changed", typeof(WorkspaceChangedHandler), Description = "Triggered when changes are made to workspaces")]
    public async Task<WebhookResponse<WorkspacesResponse>> WorkspaceChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<WorkspacesResponse, WorkspaceDto>(webhookRequest, "changed", GetWorkspacesFromPayload);

    #endregion

    #region Goals

    [Webhook("On goals added", typeof(GoalsAddedHandler), Description = "Triggered when goals are added")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<GoalsResponse, GoalResponse>(webhookRequest, "added", GetGoalsFromPayload);

    [Webhook("On goals changed", typeof(GoalsChangedHandler), Description = "Triggered when goals are changed")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<GoalsResponse, GoalResponse>(webhookRequest, "changed", GetGoalsFromPayload);

    [Webhook("On goals removed", typeof(GoalsRemovedHandler), Description = "Triggered when goals are removed")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<GoalsResponse, GoalResponse>(webhookRequest, "removed", GetGoalsFromPayload);

    [Webhook("On goals deleted", typeof(GoalsDeletedHandler), Description = "Triggered when goals are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> GoalsDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    [Webhook("On goals undeleted", typeof(GoalsUndeletedHandler), Description = "Triggered when goals are undeleted")]
    public async Task<WebhookResponse<GoalsResponse>> GoalsUndeletedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<GoalsResponse, GoalResponse>(webhookRequest, "undeleted", GetGoalsFromPayload);

    #endregion

    #region ProjectMemberships

    [Webhook("On project memberships added", typeof(ProjectMembershipsAddedHandler), Description = "Triggered when project memberships are added")]
    public async Task<WebhookResponse<ProjectMembershipsResponse>> ProjectMembershipsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectMembershipsResponse, ProjectMembershipResponse>(webhookRequest, "added", GetProjectMembershipsFromPayload);

    [Webhook("On project memberships removed", typeof(ProjectMembershipsRemovedHandler), Description = "Triggered when project memberships are removed")]
    public async Task<WebhookResponse<ProjectMembershipsResponse>> ProjectMembershipsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<ProjectMembershipsResponse, ProjectMembershipResponse>(webhookRequest, "removed", GetProjectMembershipsFromPayload);

    #endregion

    #region Teams

    [Webhook("On teams added", typeof(TeamsAddedHandler), Description = "Triggered when teams are added")]
    public async Task<WebhookResponse<TeamsResponse>> TeamsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TeamsResponse, TeamResponse>(webhookRequest, "added", GetTeamsFromPayload);

    [Webhook("On teams changed", typeof(TeamsChangedHandler), Description = "Triggered when teams are changed")]
    public async Task<WebhookResponse<TeamsResponse>> TeamsChangedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TeamsResponse, TeamResponse>(webhookRequest, "changed", GetTeamsFromPayload);

    [Webhook("On teams deleted", typeof(TeamsDeletedHandler), Description = "Triggered when teams are deleted")]
    public async Task<WebhookResponse<DeletedItemsResponse>> TeamsDeletedHandler(WebhookRequest webhookRequest) =>
        await HandleDeletionWebhookRequest<DeletedItemsResponse>(webhookRequest, "deleted");

    #endregion

    #region TeamMemberships

    [Webhook("On team memberships added", typeof(TeamMembershipsAddedHandler), Description = "Triggered when team memberships are added")]
    public async Task<WebhookResponse<TeamMembershipsResponse>> TeamMembershipsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TeamMembershipsResponse, TeamMembershipResponse>(webhookRequest, "added", GetTeamMembershipsFromPayload);

    [Webhook("On team memberships removed", typeof(TeamMembershipsRemovedHandler), Description = "Triggered when team memberships are removed")]
    public async Task<WebhookResponse<TeamMembershipsResponse>> TeamMembershipsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<TeamMembershipsResponse, TeamMembershipResponse>(webhookRequest, "removed", GetTeamMembershipsFromPayload);

    #endregion

    #region WorkspaceMemberships

    [Webhook("On workspace memberships added", typeof(WorkspaceMembershipsAddedHandler), Description = "Triggered when workspace memberships are added")]
    public async Task<WebhookResponse<WorkspaceMembershipsResponse>> WorkspaceMembershipsAddedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<WorkspaceMembershipsResponse, WorkspaceMembershipResponse>(webhookRequest, "added", GetWorkspaceMembershipsFromPayload);

    [Webhook("On workspace memberships removed", typeof(WorkspaceMembershipsRemovedHandler), Description = "Triggered when workspace memberships are removed")]
    public async Task<WebhookResponse<WorkspaceMembershipsResponse>> WorkspaceMembershipsRemovedHandler(WebhookRequest webhookRequest) =>
        await HandleWebhookRequest<WorkspaceMembershipsResponse, WorkspaceMembershipResponse>(webhookRequest, "removed", GetWorkspaceMembershipsFromPayload);

    #endregion

    #region Utils

    private WebhookResponse<T> CreatePreflightResponse<T>(string? secretKey = null) where T : class
    {
        var responseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

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

    private async Task<List<TDto>> GetEntitiesFromPayload<TDto, TRequest, TAction>(
        Payload payload, Func<InvocationContext, TAction> createAction, Func<Event, TRequest> createRequest, Func<TAction, TRequest, Task<TDto>> fetchEntity)
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
        await GetEntitiesFromPayload(payload, context => new ProjectActions(context), item => new ProjectRequest { ProjectId = item.Resource.Gid }, (action, request) => action.GetProject(request));

    private async Task<List<TaskDto>> GetTasksFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new TaskActions(context), item => new TaskRequest { TaskId = item.Resource.Gid }, (action, request) => action.GetTask(request));

    private async Task<List<TagDto>> GetTagsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new TagActions(context), item => new TagRequest { TagId = item.Resource.Gid }, (action, request) => action.GetTag(request));

    private async Task<List<AsanaEntity>> GetSectionsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new SectionActions(context), item => new SectionRequest { SectionId = item.Resource.Gid }, (action, request) => action.GetSection(request));

    private async Task<List<WorkspaceDto>> GetWorkspacesFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new WorkspaceActions(context), item => new WorkspaceRequest { WorkspaceId = item.Resource.Gid }, (action, request) => action.GetWorkspace(request));

    private async Task<List<GoalResponse>> GetGoalsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.Goals}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<GoalDto>(request).ContinueWith(task => new GoalResponse(task.Result)));

    private async Task<List<ProjectMembershipResponse>> GetProjectMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.ProjectMemberships}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<ProjectMembershipDto>(request).ContinueWith(task => new ProjectMembershipResponse(task.Result)));

    private async Task<List<StoryResponse>> GetStoriesFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.Stories}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<StoryDto>(request).ContinueWith(task => new StoryResponse(task.Result)));

    private async Task<List<TeamResponse>> GetTeamsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.Teams}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<TeamDto>(request).ContinueWith(task => new TeamResponse(task.Result)));

    private async Task<List<TeamMembershipResponse>> GetTeamMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.TeamMemberships}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<TeamMembershipDto>(request).ContinueWith(task => new TeamMembershipResponse(task.Result)));

    private async Task<List<WorkspaceMembershipResponse>> GetWorkspaceMembershipsFromPayload(Payload payload) =>
        await GetEntitiesFromPayload(payload, context => new AsanaClient(), item => new AsanaRequest($"{ApiEndpoints.WorkspaceMemberships}/{item.Resource.Gid}", Method.Get, InvocationContext.AuthenticationCredentialsProviders), (client, request) => client.ExecuteWithErrorHandling<WorkspaceMembershipDto>(request).ContinueWith(task => new WorkspaceMembershipResponse(task.Result)));

    #endregion
}
