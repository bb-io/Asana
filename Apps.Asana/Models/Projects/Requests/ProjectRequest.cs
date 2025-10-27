﻿using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.Projects.Requests;

public class ProjectRequest : WorkspaceRequest
{
    [Display("Workspace ID")]
    [DataSource(typeof(WorkspaceDataHandler))]
    public string WorkspaceId { get; set; }

    [Display("Project ID")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectId { get; set; }  

}