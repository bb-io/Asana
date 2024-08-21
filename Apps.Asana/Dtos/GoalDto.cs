using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class GoalDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("html_notes")]
    public string HtmlNotes { get; set; }

    [JsonProperty("notes")]
    public string Notes { get; set; }

    [JsonProperty("due_on")]
    public DateTime? DueOn { get; set; }

    [JsonProperty("start_on")]
    public DateTime? StartOn { get; set; }

    [JsonProperty("is_workspace_level")]
    public bool IsWorkspaceLevel { get; set; }

    [JsonProperty("liked")]
    public bool Liked { get; set; }

    [JsonProperty("likes")]
    public List<AsanaLikeDto> Likes { get; set; }

    [JsonProperty("num_likes")]
    public int NumLikes { get; set; }

    [JsonProperty("team")]
    public AsanaTeamDto Team { get; set; }

    [JsonProperty("workspace")]
    public AsanaWorkspaceDto Workspace { get; set; }

    [JsonProperty("followers")]
    public List<AsanaFollowerDto> Followers { get; set; }

    [JsonProperty("time_period")]
    public AsanaTimePeriodDto TimePeriod { get; set; }

    [JsonProperty("metric")]
    public AsanaMetricDto Metric { get; set; }

    [JsonProperty("owner")]
    public AsanaOwnerDto Owner { get; set; }

    [JsonProperty("current_status_update")]
    public AsanaStatusUpdateDto CurrentStatusUpdate { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }
}

public class AsanaLikeDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("user")]
    public AsanaUserDto User { get; set; }
}

public class AsanaUserDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaTeamDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaWorkspaceDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaFollowerDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaTimePeriodDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("end_on")]
    public DateTime? EndOn { get; set; }

    [JsonProperty("start_on")]
    public DateTime? StartOn { get; set; }

    [JsonProperty("period")]
    public string Period { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }
}

public class AsanaMetricDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }

    [JsonProperty("precision")]
    public int Precision { get; set; }

    [JsonProperty("unit")]
    public string Unit { get; set; }

    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }

    [JsonProperty("initial_number_value")]
    public double InitialNumberValue { get; set; }

    [JsonProperty("target_number_value")]
    public double TargetNumberValue { get; set; }

    [JsonProperty("current_number_value")]
    public double CurrentNumberValue { get; set; }

    [JsonProperty("current_display_value")]
    public string CurrentDisplayValue { get; set; }

    [JsonProperty("progress_source")]
    public string ProgressSource { get; set; }

    [JsonProperty("is_custom_weight")]
    public bool IsCustomWeight { get; set; }

    [JsonProperty("can_manage")]
    public bool CanManage { get; set; }
}

public class AsanaOwnerDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaStatusUpdateDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }
}
