using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Projects.Requests;

public class GetProjectStatusRequest : ProjectRequest
{
    [Display("Project status ID")]
    [DataSource(typeof(ProjectStatusDataHandler))]
    public string ProjectStatusId { get; set; }
}