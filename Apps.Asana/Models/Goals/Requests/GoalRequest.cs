using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Goals.Requests;

public class GoalRequest : WorkspaceRequest
{
    [Display("Goal ID"), DataSource(typeof(GoalDataHandler))] 
    public string GoalId { get; set; } = string.Empty;
}