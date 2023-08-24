using Apps.Asana.Constants;
using Apps.Asana.Models;
using Apps.Asana.Models.Error.Response;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Asana.Api;

public class AsanaClient : BlackBirdRestClient
{
    public AsanaClient() : base(new RestClientOptions
    {
        BaseUrl = Urls.ApiUrl.ToUri()
    })
    {
    }

    public new async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithErrorHandling(request);
        
        var data = JsonConvert.DeserializeObject<ResponseWrapper<T>>(response.Content, JsonConfig.Settings);
        return data.Data;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var errors = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        var messages = errors.Errors.Select(x => x.Message).ToArray();

        return new(string.Join("; ", messages));
    }
}