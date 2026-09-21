using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Webhooks.Models.Responses;

public class DeletedItemResponse
{
    [Display("Item ID")]
    public string ItemId { get; set; } = string.Empty;
}