using Apps.Asana.DataSourceHandlers.CustomFields.Values;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Asana.Models.CustomFields.Requests;

public class SetMultiEnumCustomFieldRequest : MultipleCustomFieldRequest
{
    [Display("Choice IDs")]
    [DataSource(typeof(MultiEnumCustomFieldValueDataHandler))]
    public IEnumerable<string>? ChoiceIds { get; set; }

    [Display("Choice names")]
    public IEnumerable<string>? ChoiceNames { get; set; }
}
