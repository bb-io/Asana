using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryCommentHandlers;

public class StoriesCommentsChangedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, ResourceSubType)
{
    const string ResourceType = "story";
    const string Action = "changed";
    const string ResourceSubType = "comment_changed";
}