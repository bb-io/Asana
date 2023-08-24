using Apps.Asana.DataSourceHandlers;
using Apps.Asana.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Projects.Requests;

public class CreateProjectRequest
{
    [Display("Team ID")] public string Team { get; set; }

    [Display("Project name")] public string Name { get; set; }

    [Display("Workspace")] 
    [DataSource(typeof(WorkspaceDataHandler))]
    public string? Workspace { get; set; }

    [Display("Is public")] public bool? Public { get; set; }

    public string? Owner { get; set; }
    
    [DataSource(typeof(ColorDataHandler))]
    public string? Color { get; set; }
    public bool? Archived { get; set; }
}