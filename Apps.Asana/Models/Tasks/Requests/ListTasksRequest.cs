using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Asana.Models.Tasks.Requests;

public class ListTasksRequest
{
    [JsonProperty("assignee")]
    [Display("Assignee ID")]
    [DataSource(typeof(UserDataHandler))]
    public string? Assignee { get; set; }
    
    [JsonProperty("section")]
    [Display("Section ID")]
    public string? Section { get; set; }
    
    [JsonProperty("tag")]
    [Display("Tag")]
    [DataSource(typeof(TagDataHandler))]
    public string? Tag { get; set; }
    
    [JsonProperty("user_task_list")]
    [Display("User task list ID")]
    public string? UserTaskList { get; set; }
}