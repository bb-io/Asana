using Apps.Asana.Constants;
using Apps.Asana.Models;
using Apps.Asana.Models.Error.Response;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Asana.Api;

public class AsanaClient : BlackBirdRestClient
{
    private const int Limit = 100;

    public AsanaClient() : base(new RestClientOptions
    {
        BaseUrl = Urls.ApiUrl.ToUri()
    })
    {
    }

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request))!.Content!;
        var val = JsonConvert.DeserializeObject<ResponseWrapper<T>>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val.Data;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
        {
            throw ConfigureErrorException(restResponse);
        }

        return restResponse;
    }

    public async Task<List<T>> Paginate<T>(RestRequest request)
    {
        var offset = string.Empty;
        var baseUrl = request.Resource;

        var result = new List<T>();
        do
        {
            request.Resource = baseUrl
                .SetQueryParameter("limit", Limit.ToString());

            if (!string.IsNullOrEmpty(offset))
                request.Resource = request.Resource
                    .SetQueryParameter("offset", offset);

            var response = await ExecuteWithErrorHandling(request);
            var responseData =
                JsonConvert.DeserializeObject<ResponseWrapper<IEnumerable<T>>>(response.Content, JsonConfig.Settings);

            result.AddRange(responseData.Data);
            offset = responseData.NextPage?.Offset;
        } while (!string.IsNullOrWhiteSpace(offset));

        return result;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (IsHtmlResponse(response))
        {
            var statusCode = (int)response.StatusCode;
            var statusDescription = response.StatusDescription ?? response.StatusCode.ToString();
            throw new PluginApplicationException(
                $"Request failed with status code {statusCode} ({statusDescription}). The server returned an HTML response instead of JSON, which typically indicates a server-side issue.");
        }

        var errors = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        var messages = errors.Errors.Select(x => x.Message).ToArray();

       throw new PluginApplicationException(string.Join("; ", messages));
    }
    
    private bool IsHtmlResponse(RestResponse response)
    {
        var contentType = response.ContentType ?? string.Empty;
        return contentType.Contains("text/html", StringComparison.OrdinalIgnoreCase);
    }
}