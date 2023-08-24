using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Sections.Requests;

public class SectionRequest
{
    [Display("Section ID")]
    public string SectionId { get; set; }
}