using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tasks.Responses;

public class ListTasksResponse
{
    public IEnumerable<AsanaEntity> Tasks { get; set; }
}