using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Asana.DataSourceHandlers;

public class TaskCustomFieldsDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] TaskRequest taskRequest)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(taskRequest.TaskId))
            throw new PluginMisconfigurationException("Please provide task ID first");

        var client = new AsanaClient();

        var endpoint = $"{ApiEndpoints.Tasks}/{taskRequest.TaskId}?opt_fields=custom_fields";
        var asanaRequest = new AsanaRequest(endpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);

        var response = await client.ExecuteWithErrorHandling<TaskDto>(asanaRequest);
        return response.CustomFields
            .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Gid, x.Name)).ToList();
    }
}
