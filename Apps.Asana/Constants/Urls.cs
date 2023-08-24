namespace Apps.Asana.Constants;

public static class Urls
{
    private const string BaseUrl = "https://app.asana.com";

    public const string ApiUrl = $"{BaseUrl}/api/1.0/";
    public const string TokenUrl = $"{BaseUrl}/-/oauth_token";
    public const string OAuthUrl = $"{BaseUrl}/-/oauth_authorize";
}