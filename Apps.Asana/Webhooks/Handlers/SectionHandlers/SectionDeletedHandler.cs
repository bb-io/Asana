using Apps.Asana.Models.Sections.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.SectionHandlers;

public class SectionDeletedHandler([WebhookParameter(true)] SectionRequest sr)
    : BaseWebhookHandler(sr.SectionId, ResourceType, Action, workspaceId: sr.WorkspaceId)
{
    const string ResourceType = "section";
    const string Action = "deleted";
}