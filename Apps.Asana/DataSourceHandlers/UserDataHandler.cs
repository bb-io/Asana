using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class UserDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Users;

    public UserDataHandler(InvocationContext invocationContext, [ActionParameter] WorkspaceRequest request) : base(invocationContext, request)
    {
    }
}