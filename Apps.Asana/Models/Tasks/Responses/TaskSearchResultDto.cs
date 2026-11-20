using Apps.Asana.Dtos;
using Apps.Asana.Dtos.Base;
using Newtonsoft.Json;

namespace Apps.Asana.Models.Tasks.Responses;

public class TaskSearchResultDto : AsanaEntity, ITaskWithParent
{
    public IEnumerable<TaskMembershipDto>? Memberships { get; set; }

    public AsanaEntity? Parent { get; set; }

    [JsonProperty("custom_fields")]
    public IEnumerable<CustomFieldDto>? CustomFields { get; set; }
}
