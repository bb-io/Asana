using System.Globalization;
using Apps.Asana.Constants;
using Apps.Asana.Models;
using Apps.Asana.Models.Entities;
using Apps.Asana.Models.Error.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.System;
using Newtonsoft.Json;

namespace Apps.Asana.Auth;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService, ITokenRefreshable
{
    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        var expiresAt = DateTime.Parse(values[CredsNames.ExpiresAt]);
        return DateTime.UtcNow > expiresAt;
    }

    public int? GetRefreshTokenExprireInMinutes(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(CredsNames.ExpiresAt, out var expireValue))
            return null;

        if (!DateTime.TryParse(expireValue, out var expireDate))
            return null;

        var difference = expireDate - DateTime.UtcNow;

        return (int)difference.TotalMinutes - 5;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var oAuthCredentials = OAuthCredentials.GetOAuthCredentials(values);
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", oAuthCredentials.ClientId },
            { "client_secret", oAuthCredentials.ClientSecret },
            { "refresh_token", values[CredsNames.RefreshToken] },
        };
        
        var response = await GetToken(bodyParameters, cancellationToken);
        response[CredsNames.RefreshToken] = values[CredsNames.RefreshToken];

        return response;
    }

    public Task<Dictionary<string, string>> RequestToken(
        string state,
        string code,
        Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var oAuthCredentials = OAuthCredentials.GetOAuthCredentials(values);
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", oAuthCredentials.ClientId },
            { "client_secret", oAuthCredentials.ClientSecret },
            { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            { "code", code },
        };

        return GetToken(bodyParameters, cancellationToken);
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> GetToken(Dictionary<string, string> parameters,
        CancellationToken token)
    {
        var tokenResponse = await ExecuteTokenRequest(parameters, token);
        var authData = DeserializeTokenResponse(tokenResponse);

        var resultDictionary = authData.AsDictionary();
        if (!resultDictionary.TryGetValue(CredsNames.ExpiresIn, out var expiresInValue) ||
            !int.TryParse(expiresInValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var expiresIn))
        {
            throw new PluginApplicationException(
                "The authorization server did not return a valid token expiration time. " +
                "Please try reconnecting; if the problem persists, contact Blackbird support.");
        }

        var expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
        resultDictionary[CredsNames.ExpiresAt] = expiresAt.ToString(CultureInfo.InvariantCulture);

        return resultDictionary;
    }

    public static AuthData DeserializeTokenResponse(TokenHttpEntity tokenEntity)
    {
        EnsureJsonContent(tokenEntity);

        if (!tokenEntity.IsSuccessStatusCode)
            throw BuildFailedRequestException(tokenEntity);

        var authData = Deserialize<AuthData>(tokenEntity);
        if (authData is null || string.IsNullOrWhiteSpace(authData.AccessToken))
            throw new PluginApplicationException(
                $"The authorization server did not return an access token (status code " +
                $"{tokenEntity.StatusDescription}): {Truncate(tokenEntity.Content!)}");

        return authData;
    }

    private static void EnsureJsonContent(TokenHttpEntity tokenEntity)
    {
        if (string.IsNullOrWhiteSpace(tokenEntity.Content))
            throw new PluginApplicationException(
                $"The authorization server returned status code {tokenEntity.StatusDescription} " +
                "without any content. Please try again later.");

        if (tokenEntity.IsHtmlContent)
            throw new PluginApplicationException(
                "The authorization server returned an HTML response instead of JSON " +
                $"(status code {tokenEntity.StatusDescription}). This usually indicates a temporary Asana " +
                "outage or an invalid token endpoint. Please try again later.");
    }

    private static PluginApplicationException BuildFailedRequestException(TokenHttpEntity tokenEntity)
    {
        var error = Deserialize<OAuthErrorResponse>(tokenEntity);
        if (error is null || string.IsNullOrWhiteSpace(error.Error))
            return new PluginApplicationException(
                $"The token request failed with status code {tokenEntity.StatusDescription}: " +
                $"{Truncate(tokenEntity.Content!)}");

        var details = string.IsNullOrWhiteSpace(error.ErrorDescription)
            ? error.Error
            : $"{error.Error}: {error.ErrorDescription}";

        return new PluginApplicationException(
            $"The authorization server rejected the token request ({details}). " +
            "Please reconnect your Asana connection.");
    }

    private static T? Deserialize<T>(TokenHttpEntity tokenEntity) where T : class
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(tokenEntity.Content!, JsonConfig.Settings);
        }
        catch (JsonException)
        {
            throw new PluginApplicationException(
                $"The authorization server returned an unexpected response (status code " +
                $"{tokenEntity.StatusDescription}): {Truncate(tokenEntity.Content!)}");
        }
    }

    private static string Truncate(string content)
    {
        const int maxLength = 500;
        var normalized = content.Trim();

        return normalized.Length <= maxLength ? normalized : normalized[..maxLength] + "...";
    }

    private async Task<TokenHttpEntity> ExecuteTokenRequest(Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        using var response = await client.PostAsync(Urls.TokenUrl, content, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return new TokenHttpEntity(response.StatusCode, response.Content.Headers.ContentType?.MediaType,
            responseContent);
    }
}