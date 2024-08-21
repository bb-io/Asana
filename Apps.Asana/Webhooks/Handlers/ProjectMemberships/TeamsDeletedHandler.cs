using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectMemberships;

public class TeamsDeletedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.GetProjectId(), ResourceType, Action)
{
    const string ResourceType = "team";
    const string Action = "removed";
}