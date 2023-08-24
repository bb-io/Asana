using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Tasks.Requests;

public class GetUserTasksRequest
{
    [Display("User task list ID")]
    public string UserTaskListId { get; set; }
}