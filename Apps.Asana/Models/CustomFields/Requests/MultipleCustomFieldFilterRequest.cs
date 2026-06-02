using Apps.Asana.DataSourceHandlers.CustomFields;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.CustomFields.Requests;


public class MultipleCustomFieldFilterRequest
{
    [Display("Custom field ID to filter by", Description = "Only multi-enum custom fields are supported.")]
    [DataSource(typeof(MultipleCustomFieldDataHandler))]
    public string? CustomFieldId { get; set; }

    [Display("Custom field value to filter by")]
    public string? CustomFieldValue { get; set; }
}
