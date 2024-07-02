using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class TeamDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("html_description")]
    public string HtmlDescription { get; set; }

    [JsonProperty("organization")]
    public AsanaOrganizationDto Organization { get; set; }

    [JsonProperty("permalink_url")]
    public string PermalinkUrl { get; set; }

    [JsonProperty("visibility")]
    public string Visibility { get; set; }

    [JsonProperty("edit_team_name_or_description_access_level")]
    public string EditTeamNameOrDescriptionAccessLevel { get; set; }

    [JsonProperty("edit_team_visibility_or_trash_team_access_level")]
    public string EditTeamVisibilityOrTrashTeamAccessLevel { get; set; }

    [JsonProperty("member_invite_management_access_level")]
    public string MemberInviteManagementAccessLevel { get; set; }

    [JsonProperty("guest_invite_management_access_level")]
    public string GuestInviteManagementAccessLevel { get; set; }

    [JsonProperty("join_request_management_access_level")]
    public string JoinRequestManagementAccessLevel { get; set; }

    [JsonProperty("team_member_removal_access_level")]
    public string TeamMemberRemovalAccessLevel { get; set; }
}

public class AsanaOrganizationDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}