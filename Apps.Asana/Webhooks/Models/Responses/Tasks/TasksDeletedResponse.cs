using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses.Tasks;

public class TasksDeletedResponse
{
    [Display("Deleted task IDs")]
    public List<string> DeletedTaskIds { get; set; }
}