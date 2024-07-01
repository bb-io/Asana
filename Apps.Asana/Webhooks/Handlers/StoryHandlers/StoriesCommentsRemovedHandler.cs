using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryHandlers;

public class StoriesCommentsRemovedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.ProjectId, ResourceType, Action, ResourceSubType)
{
    const string ResourceType = "story";
    const string Action = "removed";
    const string ResourceSubType = "comment_removed";
}