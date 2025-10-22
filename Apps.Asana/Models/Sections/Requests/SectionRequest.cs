using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Sections.Requests;

public class SectionRequest : ProjectRequest
{
    [Display("Section ID")]
    [DataSource(typeof(SectionDataHandler))]
    public string? SectionId { get; set; }
}