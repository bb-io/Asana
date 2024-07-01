using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Asana.DataSourceHandlers.Base;

public abstract class AsyncDataHandler(InvocationContext invocationContext, WorkspaceRequest workspaceRequest)
    : BaseInvocable(invocationContext), IAsyncDataSourceHandler
{
    protected IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    protected AsanaClient Client { get; } = new();

    protected abstract string Endpoint { get; }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new AsanaRequest(Endpoint, Method.Get, Creds);
        if (!string.IsNullOrEmpty(workspaceRequest.WorkspaceId))
        {
            if (!Endpoint.Contains("?project="))
            {
                request.Resource = request.Resource.SetQueryParameter("workspace", workspaceRequest.WorkspaceId);
            }
        }
        
        var items = await Client.Paginate<AsanaEntity>(request);

        return items
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.Gid, x => x.Name);
    }
}