using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.WorkspaceMemberships.Responses;

public class WorkspaceMembershipsResponse
{
    [Display("Workspace memberships")]
    public List<WorkspaceMembershipResponse> WorkspaceMemberships { get; set; } = new();
}