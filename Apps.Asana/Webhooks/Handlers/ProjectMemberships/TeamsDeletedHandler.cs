using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectMemberships;

public class TeamsDeletedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, workspaceId: pr.WorkspaceId)
{
    const string ResourceType = "team";
    const string Action = "removed";
}