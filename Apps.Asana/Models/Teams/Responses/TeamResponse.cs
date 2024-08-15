using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Teams.Responses;

public class TeamResponse
{
    [Display("Team ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("Team name")]
    public string Name { get; set; }

    [Display("Description")]
    public string Description { get; set; }

    [Display("HTML description")]
    public string HtmlDescription { get; set; }

    [Display("Organization name")]
    public string OrganizationName { get; set; }

    [Display("Permalink URL")]
    public string PermalinkUrl { get; set; }

    [Display("Visibility")]
    public string Visibility { get; set; }

    [Display("Edit team name or description access level")]
    public string EditTeamNameOrDescriptionAccessLevel { get; set; }

    [Display("Edit team visibility or trash team access level")]
    public string EditTeamVisibilityOrTrashTeamAccessLevel { get; set; }

    [Display("Member invite management access level")]
    public string MemberInviteManagementAccessLevel { get; set; }

    [Display("Guest invite management access level")]
    public string GuestInviteManagementAccessLevel { get; set; }

    [Display("Join request management access level")]
    public string JoinRequestManagementAccessLevel { get; set; }

    [Display("Team member removal access level")]
    public string TeamMemberRemovalAccessLevel { get; set; }

    public TeamResponse(TeamDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        Name = dto.Name;
        Description = dto.Description;
        HtmlDescription = dto.HtmlDescription;
        OrganizationName = dto.Organization?.Name;
        PermalinkUrl = dto.PermalinkUrl;
        Visibility = dto.Visibility;
        EditTeamNameOrDescriptionAccessLevel = dto.EditTeamNameOrDescriptionAccessLevel;
        EditTeamVisibilityOrTrashTeamAccessLevel = dto.EditTeamVisibilityOrTrashTeamAccessLevel;
        MemberInviteManagementAccessLevel = dto.MemberInviteManagementAccessLevel;
        GuestInviteManagementAccessLevel = dto.GuestInviteManagementAccessLevel;
        JoinRequestManagementAccessLevel = dto.JoinRequestManagementAccessLevel;
        TeamMemberRemovalAccessLevel = dto.TeamMemberRemovalAccessLevel;
    }
}