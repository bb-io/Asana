using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Asana
{
    public class AsanaWebhookRequest : RestRequest
    {
        public AsanaWebhookRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
        {
            var authenticationCredentialsProvider = authenticationCredentialsProviders.First(p => p.KeyName == "apiToken");
            this.AddHeader("accept", "application/json");
            this.AddHeader("content-type", "application/json");
            this.AddHeader("authorization", $"Bearer {authenticationCredentialsProvider.Value}");
        }
    }
}
