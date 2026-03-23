using Apps.Asana.Models.Teams.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TeamHandlers;

public class TeamsChangedHandler([WebhookParameter(true)] TeamRequest pr)
    : BaseWebhookHandler(pr.TeamId, ResourceType, Action, workspaceId: pr.WorkspaceId)
{
    const string ResourceType = "team";
    const string Action = "changed";
}