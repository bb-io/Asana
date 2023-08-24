using Apps.Asana.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.Actions.Base;

public class AsanaActions : BaseInvocable
{
    protected readonly AsanaClient Client;

    protected IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    protected AsanaActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new();
    }
}