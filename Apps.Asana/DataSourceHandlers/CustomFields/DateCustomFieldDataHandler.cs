using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields;

public class DateCustomFieldDataHandler : CustomFieldDataHandler
{
    protected override IEnumerable<string> FieldTypes => ["date"];

    public DateCustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] DateCustomFieldRequest request) : base(invocationContext, request)
    {
    }
}