using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields;

public class EnumCustomFieldDataHandler : CustomFieldDataHandler
{
    protected override IEnumerable<string> FieldTypes => ["enum"];

    public EnumCustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] EnumCustomFieldRequest request) : base(invocationContext, request)
    {
    }
}