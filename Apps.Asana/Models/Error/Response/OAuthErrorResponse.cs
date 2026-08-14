using Newtonsoft.Json;

namespace Apps.Asana.Models.Error.Response;

public class OAuthErrorResponse
{
    [JsonProperty("error")]
    public string? Error { get; set; }

    [JsonProperty("error_description")]
    public string? ErrorDescription { get; set; }
}
