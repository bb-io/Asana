namespace Apps.Asana.Models.Entities;

public class OAuthCredentials
{
    public string ClientId { get; init; }
    
    public string ClientSecret { get; init; }
    
    public string Scope { get; init; }

    public static OAuthCredentials GetOAuthCredentials(Dictionary<string, string> values)
    {
        var clientId = values.GetValueOrDefault("ClientId") ?? ApplicationConstants.ClientId;
        var clientSecret = values.GetValueOrDefault("ClientSecret") ?? ApplicationConstants.ClientSecret;
        var scope = values.GetValueOrDefault("Scope") ?? ApplicationConstants.Scope;
        
        return new OAuthCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope
        };
    }
}