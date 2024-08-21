using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryHandlers;

public class StoriesCommentsUndeletedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.GetProjectId(), ResourceType, Action, ResourceSubType)
{
    const string ResourceType = "story";
    const string Action = "undeleted";
    const string ResourceSubType = "comment_undeleted";
}