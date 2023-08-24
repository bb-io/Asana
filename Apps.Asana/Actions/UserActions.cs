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

[ActionList]
public class UserActions : AsanaActions
{
    public UserActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    [Action("List users", Description = "List all users")]
    public async Task<ListUsersResponse> ListUsers([ActionParameter] ListUsersRequest input)
    {
        var endpoint = ApiEndpoints.Users.WithQuery(input);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        var tasks = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Users = tasks
        };
    }

    [Action("Get user", Description = "Get user by ID")]
    public Task<UserDto> GetUser([ActionParameter] UserRequest input)
    {
        var endpoint = $"{ApiEndpoints.Users}/{input.UserId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<UserDto>(request);
    }

    [Action("Get user's task list", Description = "Get user's task list by user ID")]
    public Task<AsanaEntity> GetUserTaskList([ActionParameter] GetUserItemsRequest input)
    {
        var endpoint = $"{ApiEndpoints.Users}/{input.UserId}/user_task_list"
            .SetQueryParameter("workspace", input.WorkspaceId);
        var request = new AsanaRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<AsanaEntity>(request);
    }

    [Action("Get user's teams", Description = "Get all user's teams")]
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