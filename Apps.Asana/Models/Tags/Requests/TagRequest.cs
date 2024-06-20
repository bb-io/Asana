using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tags.Requests;

public class TagRequest
{
    [Display("Tag ID")]
    [DataSource(typeof(TagDataHandler))]
    public string TagId { get; set; }
}