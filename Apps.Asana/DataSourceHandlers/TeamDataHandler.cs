using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class TeamDataHandler : AsyncDataHandler
{    
    private readonly WorkspaceRequest _request;

    protected override string Endpoint => $"{ApiEndpoints.Workspaces}/{_request.WorkspaceId}/teams";

    public TeamDataHandler(InvocationContext invocationContext, [ActionParameter] WorkspaceRequest request) : base(invocationContext, new())
    {
        if (string.IsNullOrEmpty(request.WorkspaceId))
        {
            throw new("You should specify 'Workspace ID' first");
        }
        
        _request = request;
    }
}