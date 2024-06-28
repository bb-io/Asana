using Apps.Asana.Models.Sections.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.SectionHandlers;

public class SectionChangedHandler([WebhookParameter(true)] SectionRequest sr)
    : BaseWebhookHandler(sr.SectionId, ResourceType, Action)
{
    const string ResourceType = "section";
    const string Action = "changed";
}