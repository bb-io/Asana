using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Workspaces.Requests;

public class WorkspaceRequest : BaseWorkspaceRequest
{
    [Display("Include projects from team"), DataSource(typeof(ProjectsTeamDataHandler))]
    public string? TeamId { get; set; }
}

public class BaseWorkspaceRequest
{
    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }

    [Display("Include archived projects")]
    public bool? IncludeArchived { get; set; }
}