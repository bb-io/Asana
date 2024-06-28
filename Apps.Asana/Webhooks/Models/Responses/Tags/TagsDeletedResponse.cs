using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses.Tags;

public class TagsDeletedResponse
{
    [Display("Deleted tag IDs")]
    public List<string> DeletedTagIds { get; set; } = new ();
}