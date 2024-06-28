using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses.Sections;

public class SectionsDeletedResponse
{
    [Display("Deleted section IDs")]
    public List<string> DeletedSectionIds { get; set; }
}