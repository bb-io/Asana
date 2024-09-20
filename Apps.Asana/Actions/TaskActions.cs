using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Tags.Requests;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Asana.Actions;

[ActionList]
public class TaskActions : AsanaActions
{
    public TaskActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List tasks", Description = "List all tasks")]
    public async Task<ListTasksResponse> ListAllTasks([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ListTasksRequest input)
    {
        var projectId = projectRequest.GetProjectId();
        var endpoint = ApiEndpoints.Tasks.WithQuery(input)
            .SetQueryParameter("workspace", projectRequest.WorkspaceId);

        if (!string.IsNullOrEmpty(projectId))
            endpoint.SetQueryParameter("project", projectId);
        
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tasks = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Tasks = tasks
        };
    }

    [Action("Get task", Description = "Get task by ID")]
    public Task<TaskDto> GetTask([ActionParameter] TaskRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tasks}/{input.TaskId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<TaskDto>(request);
    }

    [Action("Create task", Description = "Create a new task")]
    public Task<TaskDto> CreateTask([ActionParameter] CreateTaskRequest input)
    {
        var payload = new ResponseWrapper<CreateTaskRequest>()
        {
            Data = input
        };

        var request = new AsanaRequest(ApiEndpoints.Tasks, Method.Post, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<TaskDto>(request);
    }

    [Action("Update task", Description = "Update task by ID")]
    public Task<TaskDto> UpdateTask(
        [ActionParameter] TaskRequest task,
        [ActionParameter] UpdateTaskRequest input)
    {
        var payload = new ResponseWrapper<UpdateTaskRequest>()
        {
            Data = input
        };

        var endpoint = $"{ApiEndpoints.Tasks}/{task.TaskId}";
        var request = new AsanaRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<TaskDto>(request);
    }

    [Action("Delete task", Description = "Delete specific task")]
    public Task DeleteTask([ActionParameter] TaskRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tasks}/{input.TaskId}";
        var request = new AsanaRequest(endpoint, Method.Delete, Creds);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Get user tasks", Description = "Get user tasks from user task list ID")]
    public async Task<ListTasksResponse> GetUserTasks([ActionParameter] GetUserTasksRequest input)
    {
        var endpoint = $"/user_task_lists/{input.UserTaskListId}{ApiEndpoints.Tasks}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tasks = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Tasks = tasks
        };
    }

    [Action("Get tasks by tag", Description = "Get tasks by specific tag")]
    public async Task<ListTasksResponse> GetTasksByTag([ActionParameter] TagRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tags}/{input.TagId}{ApiEndpoints.Tasks}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tasks = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Tasks = tasks
        };
    }

    [Action("Assign tag to task", Description = "Assign tag to a specific task")]
    public Task AssignTag(
        [ActionParameter] TaskRequest task,
        [ActionParameter] AssignTagRequest input)
    {
        var endpoint = $"{ApiEndpoints.Tasks}/{task.TaskId}/addTag";

        var payload = new ResponseWrapper<AssignTagRequest>()
        {
            Data = input
        };
        var request = new AsanaRequest(endpoint, Method.Post, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling(request);
    }
}