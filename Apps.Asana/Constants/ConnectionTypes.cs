namespace Apps.Asana.Constants;

public class ConnectionTypes
{
    public const string OAuth2 = "OAuth 2.0";
    
    public const string PersonalAccessToken = "Personal access token";
    
    public static readonly IEnumerable<string> SupportedConnectionTypes = [OAuth2, PersonalAccessToken];
}