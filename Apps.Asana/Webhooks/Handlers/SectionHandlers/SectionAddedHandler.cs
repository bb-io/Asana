using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.SectionHandlers;

public class SectionAddedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, workspaceId: pr.WorkspaceId)
{
    const string ResourceType = "section";
    const string Action = "added";
}