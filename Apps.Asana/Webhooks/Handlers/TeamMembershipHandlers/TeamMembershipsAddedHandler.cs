using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TeamMembershipHandlers;

public class TeamMembershipsAddedHandler([WebhookParameter(true)] WorkspaceRequest pr)
    : BaseWebhookHandler(pr.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "team_membership";
    const string Action = "added";
}