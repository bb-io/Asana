using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Goals.Requests;

public class GoalRequest : WorkspaceRequest
{
    [Display("Goal ID")] 
    public string GoalId { get; set; } = string.Empty;
}