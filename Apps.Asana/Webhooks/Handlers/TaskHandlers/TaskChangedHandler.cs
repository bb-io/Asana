using Apps.Asana.Webhooks.Models.Request;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Webhooks.Handlers.TaskHandlers;

public class TaskChangedHandler([WebhookParameter(true)] OnTasksChangedRequest tr) 
    : BaseWebhookHandler(tr.TaskId, ResourceType, Action, workspaceId: tr.WorkspaceId)
{
    const string ResourceType = "task";
    const string Action = "changed";

    protected override Dictionary<string, object> BuildFilter()
    {
        var filter = base.BuildFilter();

        if (tr.CustomFields != null && tr.CustomFields.Any())
            filter["fields"] = tr.CustomFields.ToArray();

        return filter;
    }

    protected override bool FilterEquals(Dictionary<string, object> a, Dictionary<string, object> b)
    {
        if (!base.FilterEquals(a, b)) 
            return false;

        var aHasFields = a.TryGetValue("fields", out var aFields);
        var bHasFields = b.TryGetValue("fields", out var bFields);

        if (!aHasFields && !bHasFields) 
            return true;
        if (aHasFields != bHasFields) 
            return false;

        var aArray = ((IEnumerable<object>)aFields!).Select(x => x.ToString()).OrderBy(x => x);
        var bArray = ((IEnumerable<object>)bFields!).Select(x => x.ToString()).OrderBy(x => x);

        return aArray.SequenceEqual(bArray);
    }
}