using Apps.Asana.Models.Tags.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TagHandlers;

public class TagChangedHandler([WebhookParameter(true)] TagRequest tr)
    : BaseWebhookHandler(tr.TagId, ResourceType, Action)
{
    const string ResourceType = "tag";
    const string Action = "changed";
}