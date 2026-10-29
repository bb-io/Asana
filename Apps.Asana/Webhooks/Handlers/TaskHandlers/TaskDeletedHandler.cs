using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TaskHandlers;

public class TaskDeletedHandler([WebhookParameter(true)] ProjectRequest projectRequest)
    : BaseWebhookHandler(projectRequest.ProjectId, ResourceType, Action, workspaceId: projectRequest.WorkspaceId)
{
    const string ResourceType = "task";
    const string Action = "deleted";
}