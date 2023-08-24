using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Tasks.Requests;

public class TaskRequest
{
    [Display("Task ID")]
    public string TaskId { get; set; }
}