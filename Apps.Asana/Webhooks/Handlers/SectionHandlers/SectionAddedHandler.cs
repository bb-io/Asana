using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.SectionHandlers;

public class SectionAddedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.GetProjectId(), ResourceType, Action)
{
    const string ResourceType = "section";
    const string Action = "added";
}