using Apps.Asana.Dtos;
using Apps.Asana.Models.Requests;
using Apps.Asana.Models.Responses;
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
    }
}
