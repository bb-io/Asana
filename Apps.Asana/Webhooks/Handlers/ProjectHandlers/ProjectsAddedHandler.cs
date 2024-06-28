namespace Apps.Asana.Webhooks.Handlers.ProjectHandlers;

public class ProjectsAddedHandler()
    : BaseWebhookHandler(string.Empty, ResourceType, Action)
{
    const string ResourceType = "project";
    const string Action = "added";
}