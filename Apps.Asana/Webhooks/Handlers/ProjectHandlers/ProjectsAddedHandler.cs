using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers;

public class ProjectsAddedHandler([WebhookParameter(true)] WorkspaceRequest r)
    : BaseWebhookHandler(r.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "project";
    const string Action = "added";
}