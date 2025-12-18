using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests
{
    public class TaskSectionRequest
    {
        [Display("Section ID")]
        [DataSource(typeof(SectionDataHandler))]
        public string? SectionId { get; set; }
    }
}
