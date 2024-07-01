using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.TeamMemberships;

public class TeamMembershipResponse
{
    [Display("Team membership ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("User name")]
    public string UserName { get; set; }

    [Display("Team name")]
    public string TeamName { get; set; }

    [Display("Guest status")]
    public bool IsGuest { get; set; }

    [Display("Limited access status")]
    public bool IsLimitedAccess { get; set; }

    [Display("Admin status")]
    public bool IsAdmin { get; set; }

    public TeamMembershipResponse(TeamMembershipDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        UserName = dto.User?.Name;
        TeamName = dto.Team?.Name;
        IsGuest = dto.IsGuest;
        IsLimitedAccess = dto.IsLimitedAccess;
        IsAdmin = dto.IsAdmin;
    }
}