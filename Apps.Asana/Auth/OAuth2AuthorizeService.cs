using Apps.Asana.Constants;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.Asana.Auth;

public class OAuth2AuthorizeService : IOAuth2AuthorizeService
{
    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var parameters = new Dictionary<string, string>
        {
            { "client_id", ApplicationConstants.ClientId },
            { "redirect_uri", ApplicationConstants.RedirectUri },
            { "response_type", "code" },
            { "scope", ApplicationConstants.Scope },
            { "state", values["state"] },
        };
        
        return Urls.OAuthUrl.WithQuery(parameters);
    }
}