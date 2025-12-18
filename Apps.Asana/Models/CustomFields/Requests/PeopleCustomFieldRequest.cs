using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.CustomFields.Requests
{
    public class PeopleCustomFieldRequest : TaskRequest
    {
        [Display("Custom field ID")]
        [DataSource(typeof(PeopleCustomFieldDataHandler))]
        public string CustomFieldId { get; set; }
    }
}
