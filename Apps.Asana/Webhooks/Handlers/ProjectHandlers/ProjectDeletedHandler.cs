using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers;

public class ProjectDeletedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.GetProjectId(), ResourceType, Action)
{
    const string ResourceType = "project";
    const string Action = "deleted";
}