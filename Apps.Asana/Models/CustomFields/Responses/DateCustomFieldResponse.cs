using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.CustomFields.Responses;

public class DateCustomFieldResponse
{
    [Display("Custom field ID")]
    public string Id { get; set; }
    
    public DateTime? Value { get; set; }
}