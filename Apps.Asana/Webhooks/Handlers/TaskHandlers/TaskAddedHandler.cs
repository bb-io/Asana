using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TaskHandlers;

public class TaskAddedHandler([WebhookParameter(true)] ProjectRequest r)
    : BaseWebhookHandler(r.ProjectId, ResourceType, Action)
{
    const string ResourceType = "task";
    const string Action = "added";
}