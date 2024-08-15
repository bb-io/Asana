using Apps.Asana.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Asana.Models.Projects.Requests;

public class UpdateProjectRequest
{
    public string? Name { get; set; }
    public bool? Archived { get; set; }
    
    [StaticDataSource(typeof(ColorDataHandler))]
    public string? Color { get; set; }
    public string? Owner { get; set; }
    
    [Display("Team ID")]
    public string? Team { get; set; }
    
    [Display("Is public")]
    public bool? Public { get; set; }
}