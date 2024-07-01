using Apps.Asana.Models.Teams.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TeamHandlers;

public class TeamsAddedHandler([WebhookParameter(true)] TeamRequest pr)
    : BaseWebhookHandler(pr.TeamId, ResourceType, Action)
{
    const string ResourceType = "team";
    const string Action = "added";
}