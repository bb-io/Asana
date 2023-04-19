using Apps.Asana.Dtos;
using Apps.Translate5;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.Asana.Models.Workspaces.Responses;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.Asana.Actions
{
    [ActionList]
    public class WorkspaceActions
    {
        [Action("List workspaces", Description = "List workspaces")]
        public ListWorkspacesResponse ListAllWorkspaces(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new AsanaClient();
            var request = new AsanaRequest($"/workspaces", Method.Get, authenticationCredentialsProviders);
            var workspaces = client.Get<ResponseWrapper<List<WorkspaceDto>>>(request);
            return new ListWorkspacesResponse()
            {
                Workspaces = workspaces.Data
            };
        }
    }
}
