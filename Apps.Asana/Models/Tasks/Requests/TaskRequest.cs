using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class TaskRequest : ProjectRequest
{
    [Display("Task ID")]
    [DataSource(typeof(TaskDataHandler))]
    public string TaskId { get; set; }
}