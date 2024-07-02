using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.ProjectMemberships.Responses;

public class ProjectMembershipsResponse
{
    [Display("Project memberships")]
    public List<ProjectMembershipResponse> ProjectMemberships { get; set; } = new();
}