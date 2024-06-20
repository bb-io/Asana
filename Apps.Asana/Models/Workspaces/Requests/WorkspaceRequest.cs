using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Workspaces.Requests;

public class WorkspaceRequest
{
    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }
}