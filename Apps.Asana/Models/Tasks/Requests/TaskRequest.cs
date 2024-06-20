using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class TaskRequest : WorkspaceRequest
{
    [Display("Project ID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectId { get; set; }
    
    [Display("Task ID")]
    [DataSource(typeof(TaskDataHandler))]
    public string TaskId { get; set; }
}