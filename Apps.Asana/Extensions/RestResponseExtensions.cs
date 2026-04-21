using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Apps.Asana.Extensions;

public static class RestResponseExtensions
{
    public static void EnsureValidJsonContent(this RestResponse response)
    {
        if (string.IsNullOrWhiteSpace(response.Content) || response.Content.Equals("undefined"))
            throw new PluginApplicationException(
                $"Status code {response.StatusCode}. The server did not return any content or it was undefined");
    }
}