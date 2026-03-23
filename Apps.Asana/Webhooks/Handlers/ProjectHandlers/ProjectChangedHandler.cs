using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers;

public class ProjectChangedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, workspaceId: pr.WorkspaceId)
{
    const string ResourceType = "project";
    const string Action = "changed";
}