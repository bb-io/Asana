using Apps.Asana.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tags.Requests;

public class UpdateTagRequest
{
    public string? Name { get; set; }
    
    [StaticDataSource(typeof(ColorDataHandler))]
    public string? Color { get; set; }
    public string? Notes { get; set; }
}