using Apps.Asana.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Asana.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        // new()
        // {
        //     Name = "OAuth2",
        //     AuthenticationType = ConnectionAuthenticationType.OAuth2,
        //     ConnectionUsage = ConnectionUsage.Actions,
        //     ConnectionProperties = new List<ConnectionProperty>
        //     {
        //         new(CredsNames.ClientId) { DisplayName = "Client ID" },
        //         new(CredsNames.ClientSecret) { DisplayName = "Client secret" },
        //         new(CredsNames.RedirectUri) { DisplayName = "Redirect URI" },
        //         new(CredsNames.Scope) { DisplayName = "Scope" },
        //         new(CredsNames.ResponseType) { DisplayName = "Response type" },
        //     }
        // },
        new()
        {
            Name = "Developer API token",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionUsage = ConnectionUsage.Webhooks,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredsNames.ApiToken) { DisplayName = "API token" }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        // var accessToken = values.First(v => v.Key == CredsNames.AccessToken);
        // yield return new AuthenticationCredentialsProvider(
        //     AuthenticationCredentialsRequestLocation.Header,
        //     accessToken.Key,
        //     accessToken.Value
        // );

        var apiToken = values.First(v => v.Key == CredsNames.ApiToken);
        yield return new AuthenticationCredentialsProvider(
            AuthenticationCredentialsRequestLocation.None,
            apiToken.Key,
            apiToken.Value
        );
    }
}