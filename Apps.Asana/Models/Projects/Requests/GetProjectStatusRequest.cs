using Apps.Asana.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Projects.Requests;

public class GetProjectStatusRequest
{
    [Display("Project ID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectId { get; set; }
    
    [Display("Project status ID")]
    [DataSource(typeof(ProjectStatusDataHandler))]
    public string ProjectStatusId { get; set; }
}