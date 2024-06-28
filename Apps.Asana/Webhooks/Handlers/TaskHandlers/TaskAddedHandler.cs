using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TaskHandlers;

public class TaskAddedHandler([WebhookParameter(true)] WorkspaceRequest r)
    : BaseWebhookHandler(r.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "task";
    const string Action = "added";
}