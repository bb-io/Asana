using Apps.Asana.Actions.Base;
using Apps.Asana.Api;
using Apps.Asana.Constants;
using Apps.Asana.DataSourceHandlers;
using Apps.Asana.Dtos;
using Apps.Asana.Models.CustomFields.Requests;
using Apps.Asana.Models.CustomFields.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dynamic;
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

    [Action("Get people custom field", Description = "Get value of a custom field with people type (returns users)")]
    public async Task<PeopleCustomFieldResponse> GetPeopleCustomField([ActionParameter] PeopleCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId);
        var field = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId)
            ?? throw new PluginApplicationException("Custom field with the provided ID was not found");

        if (!string.Equals(field.Type, "people", StringComparison.OrdinalIgnoreCase))
            throw new PluginApplicationException("Selected custom field is not of type 'people'.");

        var people = field.PeopleValue ?? Array.Empty<CompactUserDto>();

        return new PeopleCustomFieldResponse
        {
            Id = field.Gid,
            PeopleNames = people.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            PeopleIds = people.Select(x => x.Gid).Where(x => !string.IsNullOrWhiteSpace(x)).ToList()
        };
    }

    [Action("Get date custom field", Description = "Get value of a custom field with date type")]
    public async Task<DateCustomFieldResponse> GetDateCustomField([ActionParameter] DateCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId) ?? throw new PluginApplicationException("Task with the provided ID was not found");

        var customField = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId) ??
                          throw new PluginApplicationException("Custom field with the provided ID was not found");

        if (customField.DateValue == null)
        {
            return new()
            {
                Id = customField.Gid,
                Value = null
            };
        }

        DateTime? parsedDateTime = null;

        if (!string.IsNullOrEmpty(customField.DateValue.DateTime) &&
            DateTime.TryParse(customField.DateValue.DateTime, out var dt))
        {
            parsedDateTime = dt;
        }

        else if (!string.IsNullOrEmpty(customField.DateValue.Date) &&
                 DateTime.TryParse(customField.DateValue.Date, out var d))
        {
            parsedDateTime = d;
        }

        return new()
        {
            Id = customField.Gid,
            Value = parsedDateTime
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

    [Action("Get multi-enum custom field", Description = "Get values of a custom field with multi-enum type (returns option names)")]
    public async Task<MultiEnumCustomFieldResponse> GetMultiEnumCustomField([ActionParameter] MultipleCustomFieldRequest input)
    {
        var task = await GetTask(input.TaskId);
        var field = task.CustomFields.FirstOrDefault(x => x.Gid == input.CustomFieldId) ??
                          throw new PluginApplicationException("Custom field with the provided ID was not found");

        if (!string.Equals(field.Type, "multi_enum", StringComparison.OrdinalIgnoreCase))
            throw new PluginApplicationException("Selected custom field is not of type 'multi_enum'.");

        var names = (field.MultiEnumValues ?? Enumerable.Empty<CustomFieldEnumValueDto>())
            .Select(o => o.Name)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .ToList();

        return new MultiEnumCustomFieldResponse
        {
            Id = field.Gid,
            Values = names
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

    [Action("Update people custom field", Description = "Update value of a custom field with people type")]
    public Task UpdatePeopleCustomField(
    [ActionParameter] PeopleCustomFieldRequest input,
    [ActionParameter] [Display("People user IDs")] [DataSource(typeof(UserDataHandler))] IEnumerable<string> userIds)
    {
        var ids = (userIds ?? Enumerable.Empty<string>())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (ids.Length > 20)
            throw new PluginMisconfigurationException("People custom field supports up to 20 users.");

        return UpdateCustomField(input.TaskId, input.CustomFieldId, ids);
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