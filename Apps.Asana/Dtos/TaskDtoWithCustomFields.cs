using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class TaskDtoWithCustomFields : TaskDto
{
    [JsonProperty("custom_fields")]
    public IEnumerable<CustomFieldDto> CustomFields { get; set; }
}