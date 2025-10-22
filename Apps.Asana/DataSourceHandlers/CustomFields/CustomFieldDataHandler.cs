using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Dtos;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields;

public abstract class CustomFieldDataHandler : AsyncDataHandler
{
    protected abstract IEnumerable<string> FieldTypes { get; }

    protected override string Endpoint =>
        $"{ApiEndpoints.Projects}/{_request.ProjectId}/custom_field_settings";

    private readonly ProjectRequest _request;

    public CustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] ProjectRequest request) : base(invocationContext, request)
    {
        //if (string.IsNullOrEmpty(request.WorkspaceId))
        //{
        //    throw new("You should specify 'Workspace ID' first");
        //}

        _request = request;
    }

    protected override async Task<Dictionary<string, string>> GetPaginatedData(AsanaRequest request,
        DataSourceContext context)
    {
        var items = await Client.Paginate<CustomFieldSettingDto>(request);

        return items
            .Where(x => context.SearchString is null ||
                        x.CustomField.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Where(x => FieldTypes.Contains(x.CustomField.Type))
            .ToDictionary(x => x.CustomField.Gid, x => x.CustomField.Name);
    }
}