using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields;

public class NumberCustomFieldDataHandler : CustomFieldDataHandler
{
    protected override IEnumerable<string> FieldTypes => ["number"];

    public NumberCustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] DateCustomFieldRequest request) : base(invocationContext, request)
    {
    }
}