﻿using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TagHandlers;

public class TagDeletedHandler([WebhookParameter(true)] TaskRequest tr)
    : BaseWebhookHandler(tr.TaskId, ResourceType, Action)
{
    const string ResourceType = "tag";
    const string Action = "deleted";
}