using Apps.Asana.Dtos.Base;
using Newtonsoft.Json;

namespace Apps.Asana.Webhooks.Models.Payload
{
    public class WebhookSubscription : AsanaEntity
    {
        [JsonProperty("target")]
        public string Target { get; set; } = string.Empty;

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
