using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/tasks?project={input.ProjectId}", Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JArray tasksArray = content.data;
            var tasks = tasksArray.ToObject<List<TaskDto>>();
            return new ListTasksResponse()
            {
                Tasks = tasks
            };
        }

        [Action("Get task", Description = "Get task by Id")]
        public GetTaskResponse GetTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/tasks/{input.TaskId}", Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JObject taskObj = content.data;
            var task = taskObj.ToObject<TaskDto>();
            return new GetTaskResponse()
            {
                GId = task.GId,
                Name = task.Name,
            };
        }

        [Action("Update task", Description = "Update task by Id")]
        public void UpdateTask(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] UpdateTaskRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/tasks/{input.TaskId}", Method.Put, authenticationCredentialsProvider);
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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/tasks", Method.Post, authenticationCredentialsProvider);
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
            var request = new AsanaRequest($"https://app.asana.com/api/1.0/tasks/{input.TaskId}", Method.Delete, authenticationCredentialsProvider);
            client.Execute(request);
        }
    }
}
