using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class TaskActions
    {
        [Action("List tasks", Description = "List tasks")]
        public ListTasksResponse ListAllTasks(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] ListTasksRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks?project={input.ProjectId}", Method.Get, authenticationCredentialsProviders);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>>(request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Get task", Description = "Get task by Id")]
        public GetTaskResponse GetTask(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Get, authenticationCredentialsProviders);
            var task = client.Get<ResponseWrapper<TaskDto>>(request);
            return new GetTaskResponse()
            {
                GId = task.Data.GId,
                Name = task.Data.Name,
                Notes = task.Data.Notes,
            };
        }

        [Action("Update task", Description = "Update task by Id")]
        public void UpdateTask(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] UpdateTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Put, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.NewName
                }
            });

            client.Execute(request);
        }

        [Action("Create task", Description = "Create task")]
        public void CreateTask(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] CreateTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                data = new
                {
                    name = input.TaskName,
                    projects = input.ProjectId
                }
            });
            client.Execute(request);
        }

        [Action("Delete task", Description = "Delete task")]
        public void DeleteTask(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] DeleteTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Delete, authenticationCredentialsProviders);
            client.Execute(request);
        }

        [Action("Get user tasks", Description = "Get user tasks from user task list Id")]
        public ListTasksResponse GetUserTasks(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetUserTasksRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/user_task_lists/{input.UserTaskListId}/tasks", Method.Get, authenticationCredentialsProviders);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>> (request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Get tasks by tag", Description = "Get tasks by tag")]
        public ListTasksResponse GetTasksByTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetTasksByTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags/{input.TagId}/tasks", Method.Get, authenticationCredentialsProviders);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>>(request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Assign tag to task", Description = "Assign tag to task")]
        public void AssignTag(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] AssignTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}/addTag", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                data = new
                {
                    tag = input.TagId
                }
            });
            client.Execute(request);
        }
    }
}
