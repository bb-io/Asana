using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class TaskCustomFieldsRequest
{
    [Display("Custom fields"), DataSource(typeof(TaskCustomFieldsDataHandler))]
    public IEnumerable<string>? CustomFieldIds { get; set; }
}
