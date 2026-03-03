using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.CustomFields.Responses;

public class NumberCustomFieldResponse
{
    [Display("Custom field ID")]
    public string Id { get; set; }
    
    public double? Value { get; set; }
}