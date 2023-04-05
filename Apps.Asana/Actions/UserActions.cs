using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Apps.Asana.Models.Tasks.Responses;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Asana.Models.Users.Requests;
using Apps.Asana.Models.Users.Responses;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("Get user", Description = "Get user by Id")]
        public GetUserResponse GetUser(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetUserRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}", Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JObject userObj = content.data;
            var user = userObj.ToObject<UserDto>();
            return new GetUserResponse()
            {
                GId = user.GId,
                Name = user.Name,
                Email = user.Email,
                Workspaces = user.Workspaces
            };
        }

        [Action("Get user's task list", Description = "Get user's task list by user Id")]
        public GetUserTaskListResponse GetUserTaskList(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetUserTaskListRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}/user_task_list?workspace={input.WorkspaceId}", 
                Method.Get, authenticationCredentialsProvider);
            dynamic content = JsonConvert.DeserializeObject(client.Get(request).Content);
            JObject userTaskListObj = content.data;
            var userTaskList = userTaskListObj.ToObject<UserTaskListDto>();
            return new GetUserTaskListResponse()
            {
                Id = userTaskList.GId,
                Name = userTaskList.Name
            };
        }
    }
}
