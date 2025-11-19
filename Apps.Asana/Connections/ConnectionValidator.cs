using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Asana.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        /*
         * var client = new AsanaClient();
        var endpoint = ApiEndpoints.Projects.WithQuery(new {});
        var request = new AsanaRequest(endpoint, Method.Get, authProviders);
         */

        try
        {
            //await client.ExecuteWithErrorHandling<IEnumerable<AsanaEntity>>(request);
            return new() { IsValid = true, Message = "Success" };
        }
        catch (Exception ex)
        {
            return new() { IsValid = false, Message = ex.Message };
        }
    }
}