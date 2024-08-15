using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models;

public class DeletedItemsResponse
{
    [Display("Item IDs")]
    public List<string> ItemIds { get; set; } = new();
}