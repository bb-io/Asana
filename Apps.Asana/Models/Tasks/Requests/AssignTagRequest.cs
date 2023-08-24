using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class AssignTagRequest
{
    [Display("Tag")]
    [DataSource(typeof(TagDataHandler))]
    public string Tag { get; set; }
}