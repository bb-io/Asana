using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class CreateTaskRequest
{
    public string? Name { get; set; }
    public string? Notes { get; set; }
    
    [Display("Assignee ID")]
    [DataSource(typeof(UserDataHandler))]
    public string? Assignee { get; set; }
    
    [Display("Parent ID")]
    public string? Parent { get; set; }
    
    [Display("Workspace")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string? Workspace { get; set; }
    
    public IEnumerable<string>? Projects { get; set; }
}