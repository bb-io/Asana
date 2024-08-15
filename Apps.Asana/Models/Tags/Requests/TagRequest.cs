using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tags.Requests;

public class TagRequest : WorkspaceRequest
{
    [Display("Tag ID")]
    [DataSource(typeof(TagDataHandler))]
    public string TagId { get; set; }
}