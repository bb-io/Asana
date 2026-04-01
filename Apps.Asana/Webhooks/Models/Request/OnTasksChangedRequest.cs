using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.Models.Tasks.Requests;

namespace Apps.Asana.Webhooks.Models.Request;

public class OnTasksChangedRequest : TaskRequest
{
    [Display("Custom fields"), DataSource(typeof(CustomFieldDataHandler))]
    public IEnumerable<string>? CustomFields { get; set; }
}
