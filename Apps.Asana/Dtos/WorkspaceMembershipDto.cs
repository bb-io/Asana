using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class WorkspaceMembershipDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("user")]
    public AsanaUserDto User { get; set; }

    [JsonProperty("workspace")]
    public AsanaWorkspaceDto Workspace { get; set; }

    [JsonProperty("user_task_list")]
    public AsanaUserTaskListDto UserTaskList { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("is_admin")]
    public bool IsAdmin { get; set; }

    [JsonProperty("is_guest")]
    public bool IsGuest { get; set; }

    [JsonProperty("vacation_dates")]
    public AsanaVacationDatesDto VacationDates { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
}

public class AsanaUserTaskListDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("owner")]
    public AsanaUserDto Owner { get; set; }

    [JsonProperty("workspace")]
    public AsanaWorkspaceDto Workspace { get; set; }
}

public class AsanaVacationDatesDto
{
    [JsonProperty("start_on")]
    public DateTime StartOn { get; set; }

    [JsonProperty("end_on")]
    public DateTime EndOn { get; set; }
}