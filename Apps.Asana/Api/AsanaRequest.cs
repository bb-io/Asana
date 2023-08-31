using Apps.Asana.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using RestSharp;

namespace Apps.Asana.Api;

public class AsanaRequest : RestRequest
{
    public AsanaRequest(string endpoint, Method method,
        IEnumerable<AuthenticationCredentialsProvider> creds) : base(endpoint, method)
    {
        var authHeader = creds.Get(CredsNames.AccessToken);

        this.AddHeader("Authorization", $"Bearer {authHeader.Value}");
        this.AddHeader("Accept", "application/json");
    }
}