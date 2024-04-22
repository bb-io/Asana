using Apps.Asana.Api;
using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Asana.DataSourceHandlers.Base;

public abstract class AsyncDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    protected IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    protected AsanaClient Client { get; }
    
    protected abstract string Endpoint { get; }

    protected AsyncDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new();
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new AsanaRequest(Endpoint, Method.Get, Creds);
        var items = await Client.Paginate<AsanaEntity>(request);

        return items
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.Gid, x => x.Name);
    }
}