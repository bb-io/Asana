using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers;

public class UserDataHandler : AsyncDataHandler
{
    protected override string Endpoint => ApiEndpoints.Users;

    public UserDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }
}