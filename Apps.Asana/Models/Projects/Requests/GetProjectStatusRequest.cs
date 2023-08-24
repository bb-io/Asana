using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Projects.Requests;

public class GetProjectStatusRequest
{
    [Display("Project status ID")]
    public string ProjectStatusId { get; set; }
}