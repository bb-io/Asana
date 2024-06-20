using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class TagDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Tags;

    public TagDataHandler(InvocationContext invocationContext, [ActionParameter] WorkspaceRequest request) : base(invocationContext, request)
    {
    }
}