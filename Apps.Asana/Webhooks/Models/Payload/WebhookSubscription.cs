using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Webhooks.Models.Payload
{
    public class WebhookSubscription : AsanaEntity
    {
        public string Target { get; set; } = default!;
        public List<Dictionary<string, object>>? Filters { get; set; }
    }
}
