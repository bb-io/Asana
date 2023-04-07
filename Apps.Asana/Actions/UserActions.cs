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
            var user = client.Get<ResponseWrapper<UserDto>>(request);
            return new GetUserResponse()
            {
                GId = user.Data.GId,
                Name = user.Data.Name,
                Email = user.Data.Email,
                Workspaces = user.Data.Workspaces
            };
        }

        [Action("Get user's task list", Description = "Get user's task list by user Id")]
        public GetUserTaskListResponse GetUserTaskList(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetUserTaskListRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}/user_task_list?workspace={input.WorkspaceId}", 
                Method.Get, authenticationCredentialsProvider);
            var userTaskList = client.Get<ResponseWrapper<UserTaskListDto>>(request);
            return new GetUserTaskListResponse()
            {
                Id = userTaskList.Data.GId,
                Name = userTaskList.Data.Name
            };
        }

        [Action("Get user's teams", Description = "Get user's teams")]
        public GetUserTeamsResponse GetUserTeams(AuthenticationCredentialsProvider authenticationCredentialsProvider,
           [ActionParameter] GetUserTeamsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}/teams?organization={input.WorkspaceId}",
                Method.Get, authenticationCredentialsProvider);
            var teams = client.Get<ResponseWrapper<List<WorkspaceDto>>>(request);
            return new GetUserTeamsResponse()
            {
                Teams = teams.Data
            };
        }
    }
}
