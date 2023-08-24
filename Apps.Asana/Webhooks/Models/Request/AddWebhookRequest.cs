namespace Apps.Asana.Webhooks.Models.Request;

public class AddWebhookRequest
{
    public string Resource { get; set; }
    public string Target { get; set; }
    public IEnumerable<Filter> Filters { get; set; }
}