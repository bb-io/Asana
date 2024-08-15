using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Asana.Models.Projects.Requests;

public class ListProjectsRequest
{
    [JsonProperty("workspace")]
    [Display("Workspace")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string Workspace { get; set; }
    
    [JsonProperty("team")]
    [Display("Team ID")]
    public string? Team { get; set; }
    
    [JsonProperty("archived")]
    public bool? Archived { get; set; }
}