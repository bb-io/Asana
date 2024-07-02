using Apps.Asana.Dtos;

namespace Apps.Asana.Webhooks.Models.Responses.Tasks;

public class TasksResponse
{
    public List<TaskDto> Tasks { get; set; } = new();
}