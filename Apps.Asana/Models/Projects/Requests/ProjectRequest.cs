using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Asana.Models.Projects.Requests;

public class ProjectRequest : WorkspaceRequest
{
    [Display("Project ID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string? ProjectId { get; set; }  
    
    [Display("Manual project ID")]
    public string? ManualProjectId { get; set; }
    
    public string GetProjectId()
    {
        if (!(ProjectId == null ^ ManualProjectId == null))
            throw new PluginMisconfigurationException("You should specify one value: Project ID or Manual project ID");

        return ProjectId ?? ManualProjectId;
    }
}