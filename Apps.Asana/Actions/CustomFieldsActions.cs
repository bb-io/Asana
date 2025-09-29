using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.Dtos;
using Apps.Asana.Models.CustomFields.Requests;
using Apps.Asana.Models.CustomFields.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Asana.Actions;

[ActionList("Custom fields")]
public class CustomFieldsActions : AsanaActions
{
    public CustomFieldsActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get text custom field", Description = "Get value of a custom field with text type")]
    public async Task<TextCustomFieldResponse> GetTextCustomField([ActionParameter] TextCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId);
        var customField = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId) ??
                          throw new PluginApplicationException ("Custom field with the provided ID was not found");

        return new()
        {
            Id = customField.Gid,
            Value = customField.TextValue
        };
    }

    [Action("Get date custom field", Description = "Get value of a custom field with date type")]
    public async Task<DateCustomFieldResponse> GetDateCustomField([ActionParameter] DateCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId);
        var customField = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId) ??
                          throw new PluginApplicationException("Custom field with the provided ID was not found");

        return new()
        {
            Id = customField.Gid,
            Value = customField.DateValue?.DateTime
        };
    }

    [Action("Get enum custom field", Description = "Get value of a custom field with enum type")]
    public async Task<TextCustomFieldResponse> GetEnumCustomField([ActionParameter] EnumCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId);
        var customField = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId) ??
                          throw new PluginApplicationException("Custom field with the provided ID was not found");

        return new()
        {
            Id = customField.Gid,
            Value = customField.EnumValue?.Name
        };
    }


    [Action("Update text custom field", Description = "Update value of a custom field with text type")]
    public Task UpdateTextCustomField([ActionParameter] TextCustomFieldRequest input,
        [ActionParameter, Display("Value")] string value)
    {
        return UpdateCustomField(input.TaskId,  input.CustomFieldId, value);
    }

    [Action("Update date custom field", Description = "Update value of a custom field with date type")]
    public Task UpdateDateCustomField([ActionParameter] DateCustomFieldRequest input,
        [ActionParameter, Display("Value")] DateTime value)
    {
        return UpdateCustomField(input.TaskId,  input.CustomFieldId, new
        {
            date_time = value.ToString("yyyy-MM-ddTHH:mm:sszzz")
        });
    }

    [Action("Update enum custom field", Description = "Update value of a custom field with enum type")]
    public Task UpdateEnumCustomField([ActionParameter] EnumCustomFieldValueRequest input)
    {
        return UpdateCustomField(input.TaskId, input.CustomFieldId, input.EnumOptionId);
    }

    private Task<TaskDtoWithCustomFields> GetTask(string taskId)
    {
        var request = new AsanaRequest($"{ApiEndpoints.Tasks}/{taskId}?opt_fields=custom_fields", Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<TaskDtoWithCustomFields>(request);
    }

    private Task UpdateCustomField(string taskId, string customFieldId, object customFieldValue)
    {
        var request = new AsanaRequest($"{ApiEndpoints.Tasks}/{taskId}", Method.Put, Creds)
            .WithJsonBody(new
            {
                data = new
                {
                    custom_fields = new Dictionary<string, object>()
                    {
                        [customFieldId] = customFieldValue
                    }
                }
            }, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling(request);
    }
}