using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Projects.Responses;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Asana.Actions;

[ActionList("Project")]
public class ProjectActions : AsanaActions
{
    public ProjectActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search projects", Description = "List all projects")]
    public async Task<ListProjectsResponse> ListAllProjects([ActionParameter] ListProjectsRequest input)
    {
        var endpoint = ApiEndpoints.Projects.WithQuery(input);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);
        
        var projects = await Client.Paginate<AsanaEntity>(request);

        return new()
        {
            Projects = projects
        };
    }

    [Action("Get project", Description = "Get project by ID")]
    public Task<ProjectDto> GetProject([ActionParameter] ProjectRequest input)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{input.GetProjectId()}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<ProjectDto>(request);
    }

    [Action("Update project", Description = "Update project by ID")]
    public Task<ProjectDto> UpdateProject(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] UpdateProjectRequest input)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{project.GetProjectId()}";

        var payload = new ResponseWrapper<UpdateProjectRequest>
        {
            Data = input
        };
        var request = new AsanaRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<ProjectDto>(request);
    }

    [Action("Create project", Description = "Create a new project")]
    public Task<ProjectDto> CreateProject([ActionParameter] CreateProjectRequest input)
    {
        var payload = new ResponseWrapper<CreateProjectRequest>
        {
            Data = input
        };
        var request = new AsanaRequest(ApiEndpoints.Projects, Method.Post, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<ProjectDto>(request);
    }

    [Action("Delete project", Description = "Delete specific project")]
    public Task DeleteProject([ActionParameter] ProjectRequest input)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{input.GetProjectId()}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Get project sections", Description = "Get all project sections")]
    public async Task<GetProjectSectionsResponse> GetProjectSections(
        [ActionParameter] ProjectRequest input)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{input.GetProjectId()}/sections";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var sections = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Sections = sections
        };
    }

    [Action("Get project status", Description = "Get project status by ID")]
    public Task<ProjectStatusDto> GetProjectStatus([ActionParameter] GetProjectStatusRequest input)
    {
        var endpoint = $"/project_statuses/{input.ProjectStatusId}";

        var request = new AsanaRequest(endpoint, Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<ProjectStatusDto>(request);
    }

    [Action("Get project status updates", Description = "Get project all status updates")]
    public async Task<GetProjectStatusUpdatesResponse> GetProjectStatusUpdates(
        [ActionParameter] ProjectRequest input)
    {
        var endpoint = $"/status_updates?parent={input.GetProjectId()}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var updates = await Client.ExecuteWithErrorHandling<IEnumerable<ProjectStatusUpdateDto>>(request);

        return new()
        {
            Updates = updates
        };
    }

    [Action("Debug", Description = "Debug")]
    public async Task<string> Debug(
        [ActionParameter] ProjectRequest input)
    {
        var res = InvocationContext.AuthenticationCredentialsProviders.Get(CredsNames.AccessToken);
        return res.Value;
    }
}