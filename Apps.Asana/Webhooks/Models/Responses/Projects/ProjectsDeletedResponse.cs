using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses.Projects;

public class ProjectsDeletedResponse
{
    [Display("Deleted project IDs")]
    public List<string> DeletedProjectIds { get; set; }
}