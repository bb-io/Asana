using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Teams.Requests;

public class TeamRequest : WorkspaceRequest
{
    [Display("Team ID"), DataSource(typeof(TeamDataHandler))]
    public string TeamId { get; set; }
}