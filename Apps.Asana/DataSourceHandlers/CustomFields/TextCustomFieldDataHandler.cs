using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields;

public class TextCustomFieldDataHandler : CustomFieldDataHandler
{
    protected override IEnumerable<string> FieldTypes => ["text"];

    public TextCustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] TextCustomFieldRequest request) : base(invocationContext, request)
    {
    }
}