namespace Apps.Asana.Dtos;

public class TaskDtoWithCustomFields : TaskDto
{
    public IEnumerable<CustomFieldDto> CustomFields { get; set; }
}