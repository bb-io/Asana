using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers.Base;
using Apps.Asana.Dtos;
using Apps.Asana.Models.Projects.Requests;
using Apps.Asana.Models.Workspaces.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using System.Linq;

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
        _request = request;
    }

    protected override async Task<Dictionary<string, string>> GetPaginatedData(
    AsanaRequest request,
    DataSourceContext context)
    {
        // project-specific custom field settings
        var projectSettings = await Client.Paginate<CustomFieldSettingDto>(request);
        var projectFields = projectSettings
    .Select(x => new CustomFieldDto
    {
        Gid = x.CustomField.Gid,
        Name = x.CustomField.Name,
        Type = x.CustomField.Type
    });

        // workspace-level fields
        var workspaceRequest = new AsanaRequest(
           $"{ApiEndpoints.Workspaces}/{_request.WorkspaceId}/custom_fields",
            Method.Get,
            Creds
        );

        workspaceRequest.Resource = workspaceRequest.Resource.SetQueryParameter("workspace", _request.WorkspaceId);

        if (_request.TeamId != null)
            workspaceRequest.Resource = workspaceRequest.Resource.SetQueryParameter("team", _request.TeamId);

        if (!_request.IncludeArchived.GetValueOrDefault())
            workspaceRequest.Resource = workspaceRequest.Resource.SetQueryParameter("archived", "false");

        var workspaceFields = await Client.Paginate<CustomFieldDto>(workspaceRequest);

        var merged = projectFields
            .Concat(workspaceFields)
            .GroupBy(x => x.Gid)
            .Select(g => g.First())
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Where(x => FieldTypes.Contains(x.Type))
            .ToDictionary(x => x.Gid, x => x.Name);

        return merged;
    }
}