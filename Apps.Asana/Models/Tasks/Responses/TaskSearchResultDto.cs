using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tasks.Responses;

public class TaskSearchResultDto : AsanaEntity
{
    public IEnumerable<TaskMembershipDto>? Memberships { get; set; }
}
