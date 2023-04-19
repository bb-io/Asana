using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Translate5
{
    public class AsanaRequest : RestRequest
    {
        public AsanaRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
        {
            var authenticationCredentialsProvider = authenticationCredentialsProviders.First(p => p.KeyName == "apiToken");
            this.AddHeader("Authorization", $"Bearer {authenticationCredentialsProvider.Value}");
            this.AddHeader("Accept", "application/json");
        }
    }
}
