using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Tasks.Requests;

public class SubtaskFilterRequest
{
    [Display("Exclude subtasks", Description = "Return only top-level tasks and leave out subtasks. Disabled by default.")]
    public bool? ExcludeSubtasks { get; set; }
}
