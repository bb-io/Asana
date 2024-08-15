using Newtonsoft.Json;

namespace Apps.Asana.Webhooks.Models.Request;

public class AddWebhookRequest
{
    public string Resource { get; set; }
    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }
    public string Target { get; set; }
    public IEnumerable<Filter> Filters { get; set; }
}