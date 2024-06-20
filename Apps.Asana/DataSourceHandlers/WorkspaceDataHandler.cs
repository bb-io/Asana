using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class WorkspaceDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Workspaces;
    
    public WorkspaceDataHandler(InvocationContext invocationContext) : base(invocationContext, new WorkspaceRequest())
    {
    }
}