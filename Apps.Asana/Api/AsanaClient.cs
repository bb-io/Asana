﻿using Apps.Asana.Constants;
using Apps.Asana.Models;
using Apps.Asana.Models.Error.Response;
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

    public new async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithErrorHandling(request);

        var data = JsonConvert.DeserializeObject<ResponseWrapper<T>>(response.Content, JsonConfig.Settings);
        return data.Data;
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
        var errors = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        var messages = errors.Errors.Select(x => x.Message).ToArray();

        return new(string.Join("; ", messages));
    }
}