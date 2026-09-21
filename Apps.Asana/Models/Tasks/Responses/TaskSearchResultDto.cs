using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tasks.Responses;

public class TaskSearchResultDto : AsanaEntity, ITaskWithParent
{
    public IEnumerable<TaskMembershipDto>? Memberships { get; set; }

    public AsanaEntity? Parent { get; set; }
}
