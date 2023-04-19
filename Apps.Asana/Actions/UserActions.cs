using Apps.Asana.Dtos;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.Asana.Models.Users.Requests;
using Apps.Asana.Models.Users.Responses;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class UserActions
    {
        [Action("Get user", Description = "Get user by Id")]
        public GetUserResponse GetUser(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetUserRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}", Method.Get, authenticationCredentialsProviders);
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
        public GetUserTaskListResponse GetUserTaskList(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetUserTaskListRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}/user_task_list?workspace={input.WorkspaceId}", 
                Method.Get, authenticationCredentialsProviders);
            var userTaskList = client.Get<ResponseWrapper<UserTaskListDto>>(request);
            return new GetUserTaskListResponse()
            {
                Id = userTaskList.Data.GId,
                Name = userTaskList.Data.Name
            };
        }

        [Action("Get user's teams", Description = "Get user's teams")]
        public GetUserTeamsResponse GetUserTeams(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
           [ActionParameter] GetUserTeamsRequest input)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/users/{input.UserId}/teams?organization={input.WorkspaceId}",
                Method.Get, authenticationCredentialsProviders);
            var teams = client.Get<ResponseWrapper<List<WorkspaceDto>>>(request);
            return new GetUserTeamsResponse()
            {
                Teams = teams.Data
            };
        }
    }
}
