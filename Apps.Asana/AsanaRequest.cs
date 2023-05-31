using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Translate5
{
    public class AsanaRequest : RestRequest
    {
        public AsanaRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
        {
            var authenticationCredentialsProvider = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization");
            this.AddHeader("Authorization", authenticationCredentialsProvider.Value);
            this.AddHeader("Accept", "application/json");
        }
    }
}
