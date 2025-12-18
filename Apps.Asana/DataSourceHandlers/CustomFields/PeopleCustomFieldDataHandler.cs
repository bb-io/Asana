using Apps.Asana.Models.CustomFields.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Asana.DataSourceHandlers.CustomFields
{
    public class PeopleCustomFieldDataHandler : CustomFieldDataHandler
    {
        protected override IEnumerable<string> FieldTypes => ["people"];

        public PeopleCustomFieldDataHandler(InvocationContext invocationContext, [ActionParameter] PeopleCustomFieldRequest request)
            : base(invocationContext, request) { }
    }
}
