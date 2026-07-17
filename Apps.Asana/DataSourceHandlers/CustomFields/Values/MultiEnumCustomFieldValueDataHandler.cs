using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Dtos;
using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields.Values;

public class MultiEnumCustomFieldValueDataHandler : AsyncDataHandler
{
    protected override string Endpoint =>
        $"{ApiEndpoints.CustomFields}/{_request.CustomFieldId}";

    private readonly SetMultiEnumCustomFieldRequest _request;

    public MultiEnumCustomFieldValueDataHandler(InvocationContext invocationContext,
        [ActionParameter] SetMultiEnumCustomFieldRequest request) : base(invocationContext, request)
    {
        if (string.IsNullOrEmpty(request.WorkspaceId))
            throw new("You should specify 'Workspace ID' first");

        if (string.IsNullOrEmpty(request.CustomFieldId))
            throw new("You should specify 'Custom field ID' first");

        _request = request;
    }

    protected override async Task<Dictionary<string, string>> GetPaginatedData(AsanaRequest request,
        DataSourceContext context)
    {
        var customField = await Client.ExecuteWithErrorHandling<CustomFieldDto>(request);

        return (customField.EnumOptions ?? Enumerable.Empty<CustomFieldEnumValueDto>())
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Gid, x => x.Name);
    }
}
