using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.CustomFields.Responses;

public class TextCustomFieldResponse
{
    [Display("Custom field ID")]
    public string Id { get; set; }
    
    public string? Value { get; set; }
}