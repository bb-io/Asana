using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryCommentHandlers;

public class StoriesCommentsAddedHandler([WebhookParameter(true)] WorkspaceRequest pr)
    : BaseWebhookHandler(pr.WorkspaceId, ResourceType, Action, ResourceSubType)
{
    const string ResourceType = "story";
    const string Action = "added";
    const string ResourceSubType = "comment_added";
}