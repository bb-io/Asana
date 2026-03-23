using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TagHandlers;

public class TagAddedHandler([WebhookParameter(true)] TaskRequest r)
    : BaseWebhookHandler(r.TaskId, ResourceType, Action, workspaceId: r.WorkspaceId)
{
    const string ResourceType = "tag";
    const string Action = "added";
}