using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Webhooks.Models.Request;

public class OnTasksChangedRequest : TaskRequest
{
    [Display("Custom fields"), DataSource(typeof(TaskCustomFieldsDataHandler))]
    public IEnumerable<string>? CustomFieldIds { get; set; }
}
