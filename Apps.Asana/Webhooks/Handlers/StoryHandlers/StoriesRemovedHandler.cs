using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryHandlers;

public class StoriesRemovedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, workspaceId: pr.WorkspaceId)
{
    const string ResourceType = "story";
    const string Action = "removed";
}