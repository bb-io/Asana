using Apps.Asana.DataSourceHandlers;
using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.DataSourceHandlers.CustomFields.Values;
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

    [Display("Text custom fields ID")]
    [DataSource(typeof(TextCustomFieldDataHandler))]
    public string? TextCustomFieldId { get; set; }

    [Display("Text custom fields contains")]
    public string? TextCustomFieldContains { get; set; }

    [Display("Enum custom fields ID")]
    [DataSource(typeof(EnumCustomFieldDataHandler))]
    public string? EnumCustomFieldId { get; set; }

    [DataSource(typeof(EnumCustomFieldValueDataHandler))]
    [Display("Enum option ID")]
    public string? EnumOptionId { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }

    [Display("Modified after")]
    public DateTime? ModifiedAfter { get; set; }

    [Display("Modified before")]
    public DateTime? ModifiedBefore { get; set; }
}