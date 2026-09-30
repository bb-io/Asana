using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Workspaces.Requests;
using RestSharp;
using Apps.Asana.Models.Workspaces.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.Actions;

[ActionList("Workspace")]
public class WorkspaceActions(InvocationContext invocationContext) : AsanaActions(invocationContext)
{
    [Action("Search workspaces", Description = "Search workspaces accessible to the connected user and output their IDs and names.")]
    public async Task<ListWorkspacesResponse> ListAllWorkspaces()
    {
        var request = new AsanaRequest(ApiEndpoints.Workspaces, Method.Get, Creds);
        var workspaces = await Client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);

        return new()
        {
            Workspaces = workspaces
        };
    }

    [Action("Get workspace", Description = "Output the details of the selected workspace.")]
    public Task<WorkspaceDto> GetWorkspace([ActionParameter] WorkspaceRequest workspace)
    {
        var endpoint = $"{ApiEndpoints.Workspaces}/{workspace.WorkspaceId}";
        var request = new AsanaRequest(endpoint, Method.Get, Creds);
        
        return Client.ExecuteWithErrorHandling<WorkspaceDto>(request);
    }
}