using System.Globalization;
using Apps.Asana.Constants;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Newtonsoft.Json;

namespace Apps.Asana.Auth;

public class OAuth2TokenService : IOAuth2TokenService
{
    private const string ExpiresAtKeyName = "expires_at";

    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        var expiresAt = DateTime.Parse(values[ExpiresAtKeyName]);
        return DateTime.UtcNow > expiresAt;
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", values[CredsNames.ClientId] },
            { "client_secret", values[CredsNames.ClientSecret] },
            { "refresh_token", values["refresh_token"] },
        };
        return GetToken(bodyParameters, cancellationToken);
    }

    public Task<Dictionary<string, string>> RequestToken(
        string state,
        string code,
        Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", values[CredsNames.ClientId] },
            { "client_secret", values[CredsNames.ClientSecret] },
            { "redirect_uri", values[CredsNames.RedirectUri] },
            { "code", code }
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
        var responseContent = await ExecuteTokenRequest(parameters, token);

        var resultDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent)
                                   ?.ToDictionary(r => r.Key, r => r.Value.ToString())
                               ?? throw new InvalidOperationException(
                                   $"Invalid response content: {responseContent}");

        var expiresIn = int.Parse(resultDictionary["expires_in"]);
        var expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
        resultDictionary.Add(ExpiresAtKeyName, expiresAt.ToString(CultureInfo.InvariantCulture));

        return resultDictionary;
    }

    private async Task<string> ExecuteTokenRequest(Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        using var response = await client.PostAsync(Urls.TokenUrl, content, cancellationToken);

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}