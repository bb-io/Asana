using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectMemberships;

public class ProjectMembershipsRemovedHandler([WebhookParameter(true)] WorkspaceRequest pr)
    : BaseWebhookHandler(pr.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "project_membership";
    const string Action = "removed";
}