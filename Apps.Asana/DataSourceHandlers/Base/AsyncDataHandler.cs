using Apps.Asana.Api;
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

    public Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = PrepareRequest();
        return GetPaginatedData(request, context);
    }

    protected virtual async Task<Dictionary<string, string>> GetPaginatedData(AsanaRequest request,
        DataSourceContext context)
    {
        var items = await Client.Paginate<AsanaEntity>(request);

        return items
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Gid, x => x.Name);
    }

    protected virtual AsanaRequest PrepareRequest()
    {
        var request = new AsanaRequest(Endpoint, Method.Get, Creds);
        if (string.IsNullOrEmpty(workspaceRequest.WorkspaceId) || Endpoint.Contains("?project="))
            return request;

        request.Resource = request.Resource.SetQueryParameter("workspace", workspaceRequest.WorkspaceId);

        if (!workspaceRequest.IncludeArchived.HasValue || !workspaceRequest.IncludeArchived.Value)
        {
            request.Resource = request.Resource.SetQueryParameter("archived", "false");
        }

        if (workspaceRequest.TeamId != null)
        {
            request.Resource = request.Resource.SetQueryParameter("team", workspaceRequest.TeamId);
        }

        return request;
    }
}