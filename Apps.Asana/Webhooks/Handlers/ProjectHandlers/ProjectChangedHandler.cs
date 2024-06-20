using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers;

public class ProjectChangedHandler : BaseWebhookHandler
{
    const string ResourceType = "project";
    const string Action = "changed";

    public ProjectChangedHandler([WebhookParameter(true)] ProjectRequest pr) : base(pr.ProjectId, ResourceType, Action)
    {
    }
}