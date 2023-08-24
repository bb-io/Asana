using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Asana.Models.Tags.Requests;

public class ListTagsRequest
{
    [JsonProperty("workspace")]
    [Display("Workspace")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string? WorkspaceId { get; set; }
}