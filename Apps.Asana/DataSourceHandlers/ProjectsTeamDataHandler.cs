using Apps.Asana.Constants;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.Asana.Api;
using Apps.Asana.Dtos.Base;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.DataSourceHandlers
{
    public class ProjectsTeamDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private readonly BaseWorkspaceRequest _request;

        protected AsanaClient Client { get; } = new();

        public ProjectsTeamDataHandler(InvocationContext invocationContext, [ActionParameter] BaseWorkspaceRequest request) : base(invocationContext)
        {
            _request = request;
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_request.WorkspaceId))
            {
                throw new("You should specify 'Workspace ID' first");
            }
            string endpoint = $"{ApiEndpoints.Workspaces}/{_request.WorkspaceId}/teams";
            var request = new AsanaRequest(endpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);

            var items = await Client.Paginate<AsanaEntity>(request);

            return items
                .Where(x => context.SearchString is null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Gid, x => x.Name);
        }
    }
}
