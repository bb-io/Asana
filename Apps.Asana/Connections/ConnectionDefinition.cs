using Apps.Asana.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Asana.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = ConnectionTypes.OAuth2,
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties = new List<ConnectionProperty>()
        },
        new()
        {
            Name = ConnectionTypes.PersonalAccessToken,
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredsNames.AccessToken)
                {
                    DisplayName = "Personal access token",
                    Description = "Asana Personal Access Token. You can create it in your Asana account settings.",
                    Sensitive = true
                }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        var credentials = values.Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();
        var connectionType = values[nameof(ConnectionPropertyGroup)] switch
        {
            var ct when ConnectionTypes.SupportedConnectionTypes.Contains(ct) => ct,
            _ => throw new Exception($"Unknown connection type: {values[nameof(ConnectionPropertyGroup)]}")
        };
        
        credentials.Add(new AuthenticationCredentialsProvider(CredsNames.ConnectionType, connectionType));
        return credentials;
    }
}