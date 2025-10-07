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
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Asana.Actions;

[ActionList("Task")]
public class TaskActions : AsanaActions
{
    public TaskActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search tasks", Description = "List all tasks")]
    public async Task<ListTasksResponse> ListAllTasks([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ListTasksRequest input)
    {
        if (input.CreatedAfter.HasValue && input.CreatedBefore.HasValue &&
            input.CreatedAfter > input.CreatedBefore)
            throw new PluginMisconfigurationException("'Created after' must be earlier than 'Created before'.");

        if (input.ModifiedAfter.HasValue && input.ModifiedBefore.HasValue &&
            input.ModifiedAfter > input.ModifiedBefore)
            throw new PluginMisconfigurationException("'Modified after' must be earlier than 'Modified before'.");

        if (!string.IsNullOrWhiteSpace(input.TextCustomFieldContains) &&
            string.IsNullOrWhiteSpace(input.TextCustomFieldId))
            throw new PluginMisconfigurationException("Provide 'Text custom fields ID' when using 'Text custom fields contains'.");

        if (!string.IsNullOrWhiteSpace(input.EnumOptionId) &&
            string.IsNullOrWhiteSpace(input.CustomFieldId))
            throw new PluginMisconfigurationException("Provide 'Enum custom fields ID' when using 'Enum option ID'.");

        string endpoint = $"/workspaces/{projectRequest.WorkspaceId}/tasks/search";

        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        static void AddIf(RestRequest r, string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                r.AddQueryParameter(name, value);
        }
        static string IsoUtc(DateTime dt) => dt.ToUniversalTime().ToString("o");

        AddIf(request, "projects.any", projectRequest.ProjectId);
        AddIf(request, "assignee.any", input.Assignee);
        AddIf(request, "tags.any", input.Tag);
        AddIf(request, "sections.any", input.Section);
        AddIf(request, "user_task_lists.any", input.UserTaskList);

        if (input.CreatedAfter.HasValue) request.AddQueryParameter("created_at.after", IsoUtc(input.CreatedAfter.Value));
        if (input.CreatedBefore.HasValue) request.AddQueryParameter("created_at.before", IsoUtc(input.CreatedBefore.Value));
        if (input.ModifiedAfter.HasValue) request.AddQueryParameter("modified_at.after", IsoUtc(input.ModifiedAfter.Value));
        if (input.ModifiedBefore.HasValue) request.AddQueryParameter("modified_at.before", IsoUtc(input.ModifiedBefore.Value));

        if (!string.IsNullOrWhiteSpace(input.TextCustomFieldId) &&
            !string.IsNullOrWhiteSpace(input.TextCustomFieldContains))
        {
            request.AddQueryParameter(
                $"custom_fields.{input.TextCustomFieldId}.text_value.contains",
                input.TextCustomFieldContains);
        }

        if (!string.IsNullOrWhiteSpace(input.CustomFieldId) &&
            !string.IsNullOrWhiteSpace(input.EnumOptionId))
        {
            request.AddQueryParameter(
                $"custom_fields.{input.CustomFieldId}.enum_value",
                input.EnumOptionId);
        }
        request.AddQueryParameter("opt_fields",
            "gid,name,assignee.gid,projects.gid,created_at,modified_at,custom_fields,custom_fields.enum_value.gid,custom_fields.text_value");

        var tasks = await Client.Paginate<AsanaEntity>(request);

        return new ListTasksResponse { Tasks = tasks };
    }

    private static string IsoUtc(DateTime dt) => dt.ToUniversalTime().ToString("o");

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