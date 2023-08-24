using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class TagDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Tags;

    public TagDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }
}