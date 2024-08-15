using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.WorkspaceHandlers;

public class WorkspaceChangedHandler([WebhookParameter(true)] WorkspaceRequest r)
    : BaseWebhookHandler(r.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "workspace";
    const string Action = "changed";
}