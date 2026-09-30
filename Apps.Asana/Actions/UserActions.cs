using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.Asana.Models.Users.Requests;
using Apps.Asana.Models.Users.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.Asana.Actions;

[ActionList("User")]
public class UserActions(InvocationContext invocationContext) : AsanaActions(invocationContext)
{
    [Action("Search users", Description = "Search users, optionally by workspace or team, and output their IDs and names.")]
    public async Task<ListUsersResponse> ListUsers([ActionParameter] ListUsersRequest input)
    {
        string endpoint = ApiEndpoints.Users.WithQuery(input);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tasks = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Users = tasks
        };
    }

    [Action("Get user", Description = "Output the details of the selected user.")]
    public Task<UserDto> GetUser([ActionParameter] UserRequest input)
    {
        var endpoint = $"{ApiEndpoints.Users}/{input.UserId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<UserDto>(request);
    }

    [Action("Get user's task list", Description = "Output the ID and name of the selected user's My Tasks list in the selected workspace.")]
    public Task<AsanaEntity> GetUserTaskList([ActionParameter] GetUserItemsRequest input)
    {
        var endpoint = $"{ApiEndpoints.Users}/{input.UserId}/user_task_list"
            .SetQueryParameter("workspace", input.WorkspaceId);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<AsanaEntity>(request);
    }

    [Action("Get user's teams", Description = "Output the IDs and names of teams the selected user belongs to in the selected workspace.")]
    public async Task<GetUserTeamsResponse> GetUserTeams([ActionParameter] GetUserItemsRequest input)
    {
        var endpoint = $"{ApiEndpoints.Users}/{input.UserId}/teams"
            .SetQueryParameter("workspace", input.WorkspaceId);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);
        
        var teams = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);
        
        return new()
        {
            Teams = teams
        };
    }
}