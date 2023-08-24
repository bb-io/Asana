using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Sections.Requests;

public class ManageSectionRequest
{
    public string Name { get; set;}
    
    [Display("Insert before")]
    public string? InsertBefore { get; set;}
    
    [Display("Insert after")]
    public string? InsertAfter { get; set;}
}