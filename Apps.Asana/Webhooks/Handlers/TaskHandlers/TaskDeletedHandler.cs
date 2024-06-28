﻿using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TaskHandlers;

public class TaskDeletedHandler([WebhookParameter(true)] TaskRequest tr)
    : BaseWebhookHandler(tr.TaskId, ResourceType, Action)
{
    const string ResourceType = "task";
    const string Action = "deleted";
}