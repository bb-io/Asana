using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses;

public class ProjectsDeletedResponse
{
    [Display("Deleted Project IDs")]
    public List<string> DeletedProjectIds { get; set; }
}