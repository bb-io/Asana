﻿using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.GoalHandlers;

public class GoalsDeletedHandler([WebhookParameter(true)] WorkspaceRequest pr)
    : BaseWebhookHandler(pr.WorkspaceId, ResourceType, Action)
{
    const string ResourceType = "goal";
    const string Action = "deleted";
}