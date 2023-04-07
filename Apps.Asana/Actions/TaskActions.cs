using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class TaskActions
    {
        [Action("List tasks", Description = "List tasks")]
        public ListTasksResponse ListAllTasks(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] ListTasksRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks?project={input.ProjectId}", Method.Get, authenticationCredentialsProvider);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>>(request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Get task", Description = "Get task by Id")]
        public GetTaskResponse GetTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Get, authenticationCredentialsProvider);
            var task = client.Get<ResponseWrapper<TaskDto>>(request);
            return new GetTaskResponse()
            {
                GId = task.Data.GId,
                Name = task.Data.Name,
                Notes = task.Data.Notes,
            };
        }

        [Action("Update task", Description = "Update task by Id")]
        public void UpdateTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UpdateTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Put, authenticationCredentialsProvider);
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
        public void CreateTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] CreateTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks", Method.Post, authenticationCredentialsProvider);
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
        public void DeleteTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] DeleteTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }

        [Action("Get user tasks", Description = "Get user tasks from user task list Id")]
        public ListTasksResponse GetUserTasks(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetUserTasksRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/user_task_lists/{input.UserTaskListId}/tasks", Method.Get, authenticationCredentialsProvider);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>> (request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Get tasks by tag", Description = "Get tasks by tag")]
        public ListTasksResponse GetTasksByTag(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetTasksByTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tags/{input.TagId}/tasks", Method.Get, authenticationCredentialsProvider);
            var tasks = client.Get<ResponseWrapper<List<TaskDto>>>(request);
            return new ListTasksResponse()
            {
                Tasks = tasks.Data
            };
        }

        [Action("Assign tag to task", Description = "Assign tag to task")]
        public void AssignTag(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] AssignTagRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/tasks/{input.TaskId}/addTag", Method.Post, authenticationCredentialsProvider);
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
