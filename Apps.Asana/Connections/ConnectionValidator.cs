using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Asana.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        var client = new AsanaClient();
        var endpoint = ApiEndpoints.Projects.WithQuery(new {});
        var request = new AsanaRequest(endpoint, Method.Get, authProviders);

        try
        {
            await client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);
            return new() { IsValid = true, Message = "Success" };
        }
        catch (Exception ex)
        {
            return new() { IsValid = false, Message = ex.Message };
        }
    }
}