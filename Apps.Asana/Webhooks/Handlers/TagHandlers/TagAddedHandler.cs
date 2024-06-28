using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TagHandlers;

public class TagAddedHandler([WebhookParameter(true)] WorkspaceRequest r)
    : BaseWebhookHandler(r.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "tag";
    const string Action = "added";
}