using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class TeamMembershipDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("user")]
    public AsanaUserDto User { get; set; }

    [JsonProperty("team")]
    public AsanaTeamDto Team { get; set; }

    [JsonProperty("is_guest")]
    public bool IsGuest { get; set; }

    [JsonProperty("is_limited_access")]
    public bool IsLimitedAccess { get; set; }

    [JsonProperty("is_admin")]
    public bool IsAdmin { get; set; }
}
