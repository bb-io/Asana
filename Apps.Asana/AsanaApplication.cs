using Apps.Asana.Auth;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Asana;

public class AsanaApplication : BaseInvocable, IApplication, ICategoryProvider
{
    private string _name;
    private readonly Dictionary<Type, object> _typesInstances;

    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.ProjectManagementAndProductivity, ApplicationCategory.TaskManagement];
        set { }
    }

    public AsanaApplication(InvocationContext invocationContext) : base(invocationContext)
    {
        _name = "Asana";
        _typesInstances = CreateTypesInstances();
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public T GetInstance<T>()
    {
        if (!_typesInstances.TryGetValue(typeof(T), out var value))
        {
            throw new InvalidOperationException($"Instance of type '{typeof(T)}' not found");
        }

        return (T)value;
    }

    private Dictionary<Type, object> CreateTypesInstances()
    {
        return new Dictionary<Type, object>
        {
            { typeof(IOAuth2AuthorizeService), new OAuth2AuthorizeService(InvocationContext) },
            { typeof(IOAuth2TokenService), new OAuth2TokenService(InvocationContext) }
        };
    }
}