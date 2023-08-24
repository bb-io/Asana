using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class ProjectDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Projects;

    public ProjectDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }
}