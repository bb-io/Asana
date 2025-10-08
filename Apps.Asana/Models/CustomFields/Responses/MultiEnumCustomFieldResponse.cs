using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.CustomFields.Responses
{
    public class MultiEnumCustomFieldResponse
    {
        [Display("Custom field ID")]
        public string Id { get; set; } = default!;

        [Display("Option names")]
        public IEnumerable<string> Values { get; set; } = Array.Empty<string>();
    }
}
