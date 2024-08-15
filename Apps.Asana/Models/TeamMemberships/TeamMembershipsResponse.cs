using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.TeamMemberships;

public class TeamMembershipsResponse
{
    [Display("Team memberships")]
    public List<TeamMembershipResponse> TeamMemberships { get; set; } = new();
}