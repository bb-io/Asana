using Apps.Asana.DataSourceHandlers;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.DataSourceHandlers.CustomFields.Values;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.CustomFields.Requests;

public class EnumCustomFieldValueRequest : TaskRequest
{
    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }

    [Display("Custom field"), DataSource(typeof(EnumCustomFieldDataHandler))]
    public string CustomFieldId { get; set; }  
    
    [Display("Enum value"), DataSource(typeof(EnumCustomFieldValueDataHandler))]
    public string EnumOptionId { get; set; }
}