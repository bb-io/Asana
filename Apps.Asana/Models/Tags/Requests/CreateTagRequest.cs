using Apps.Asana.DataSourceHandlers.EnumDataHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Asana.Models.Tags.Requests;

public class CreateTagRequest : WorkspaceRequest
{
    public string? Name { get; set; }
    
    [StaticDataSource(typeof(ColorDataHandler))]
    public string? Color { get; set; }
    public string? Notes { get; set; }
}