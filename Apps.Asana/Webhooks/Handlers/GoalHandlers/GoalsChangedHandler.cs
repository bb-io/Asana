using Apps.Asana.Models.Goals.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.GoalHandlers;

public class GoalsChangedHandler([WebhookParameter(true)] GoalRequest pr)
    : BaseWebhookHandler(pr.GoalId, ResourceType, Action)
{
    const string ResourceType = "goal";
    const string Action = "changed";
}