using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.StoryHandlers;

public class StoriesAddedHandler([WebhookParameter(true)] ProjectRequest pr)
    : BaseWebhookHandler(pr.GetProjectId(), ResourceType, Action)
{
    const string ResourceType = "story";
    const string Action = "added";
}