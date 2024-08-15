using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class GoalDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Goals;

    public GoalDataHandler(InvocationContext invocationContext, [ActionParameter] WorkspaceRequest request) : base(invocationContext, request)
    {
        if (string.IsNullOrEmpty(request.WorkspaceId))
        {
            throw new("You should specify 'Workspace ID' first");
        }
    }
}