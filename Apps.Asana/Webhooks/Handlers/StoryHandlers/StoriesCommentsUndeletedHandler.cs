using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryHandlers;

public class StoriesCommentsUndeletedHandler([WebhookParameter(true)] WorkspaceRequest pr)
    : BaseWebhookHandler(pr.WorkspaceId, ResourceType, Action, ResourceSubType)
{
    const string ResourceType = "story";
    const string Action = "undeleted";
    const string ResourceSubType = "comment_undeleted";
}