using Apps.Asana.Constants;

namespace Apps.Asana.Models.Entities;

public class OAuthCredentials
{
    public string ClientId { get; init; }
    
    public string ClientSecret { get; init; }
    
    public string Scope { get; init; }

    public static OAuthCredentials GetOAuthCredentials(Dictionary<string, string> values)
    {
        var clientId = values.GetValueOrDefault(CredsNames.OwnAppClientId) ?? ApplicationConstants.ClientId;
        var clientSecret = values.GetValueOrDefault(CredsNames.OwnAppClientSecret) ?? ApplicationConstants.ClientSecret;
        var scope = values.GetValueOrDefault(CredsNames.OwnAppScopes) ?? ApplicationConstants.Scope;
        
        return new OAuthCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
        };
    }
}