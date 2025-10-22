using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class TaskDataHandler : AsyncDataHandler
{
    protected override string Endpoint => $"/tasks?project={_request.ProjectId}";

    private readonly TaskRequest _request;

    public TaskDataHandler(InvocationContext invocationContext,
        [ActionParameter] TaskRequest request) : base(invocationContext, request)
    {
        if (string.IsNullOrEmpty(request.WorkspaceId))
        {
            throw new("You should specify 'Project ID' or 'Manual project ID'");
        }
        _request = request;
    }
}