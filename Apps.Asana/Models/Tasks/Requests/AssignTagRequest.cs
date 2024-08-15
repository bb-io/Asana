using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class AssignTagRequest : WorkspaceRequest
{
    [Display("Tag")]
    [DataSource(typeof(TagDataHandler))]
    public string Tag { get; set; }
}