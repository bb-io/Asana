using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields
{
    public class MultipleCustomFieldDataHandler : CustomFieldDataHandler
    {
        protected override IEnumerable<string> FieldTypes => new[] { "multi_enum" };

        public MultipleCustomFieldDataHandler(InvocationContext invocationContext,
        [ActionParameter] MultipleCustomFieldRequest request) : base(invocationContext, request)
        {
        }
    }
}
