using Apps.Asana.Models.Tags.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TagHandlers;

public class TagUndeletedHandler([WebhookParameter(true)] TagRequest tr)
    : BaseWebhookHandler(tr.TagId, ResourceType, Action)
{
    const string ResourceType = "tag";
    const string Action = "undeleted";
}