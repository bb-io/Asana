using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.CustomFields.Responses
{
    public class PeopleCustomFieldResponse
    {
        [Display("Custom field ID")]
        public string Id { get; set; }

        [Display("People names")]
        public List<string> PeopleNames { get; set; } = new();

        [Display("People IDs")]
        public List<string> PeopleIds { get; set; } = new();
    }
}
