using Apps.Asana.DataSourceHandlers;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.CustomFields.Requests;

public class EnumCustomFieldRequest : TaskRequest
{
    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }

    [Display("Custom field"), DataSource(typeof(EnumCustomFieldDataHandler))]
    public string CustomFieldId { get; set; }
}