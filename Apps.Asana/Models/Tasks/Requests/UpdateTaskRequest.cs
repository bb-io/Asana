using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Tasks.Requests;

public class UpdateTaskRequest
{
    public string? Name { get; set; }
    public string? Notes { get; set; }
    
    [Display("Assignee ID")]
    [DataSource(typeof(UserDataHandler))]
    public string? Assignee { get; set; }
}