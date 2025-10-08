using Apps.Asana.DataSourceHandlers.CustomFields;
using Apps.Asana.Models.Tasks.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Asana.Models.CustomFields.Requests
{
    public class MultipleCustomFieldRequest : TaskRequest
    {
        [Display("Custom field ID")]
        [DataSource(typeof(MultipleCustomFieldDataHandler))]
        [JsonProperty("custom_field_id")]
        public string CustomFieldId { get; set; } = default!;
    }
}
