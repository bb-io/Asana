using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class ProjectStatusDataHandler : AsyncDataHandler
{
    protected override string Endpoint => $"projects/{_request.GetProjectId()}/project_statuses";

    private readonly GetProjectStatusRequest _request;

    public ProjectStatusDataHandler(InvocationContext invocationContext,
        [ActionParameter] GetProjectStatusRequest request) : base(invocationContext, request)
    {
        _request = request;
    }
}