using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Translate5
{
    public class AsanaClient : RestClient
    {
        public AsanaClient() : base(new RestClientOptions() { ThrowOnAnyError = true, BaseUrl = new Uri("https://app.asana.com/api/1.0/") }) { }
    }
}
