using Apps.Asana.Dtos.Base;
using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class CustomFieldDto : AsanaEntity
{
    public string Type { get; set; }
    public string TextValue { get; set; }

    [JsonProperty("enum_options")]
    public IEnumerable<CustomFieldEnumValueDto>? EnumOptions { get; set; }

    [JsonProperty("enum_value")]
    public CustomFieldEnumValueDto? EnumValue { get; set; }

    [JsonProperty("date_value")]
    public CustomFieldDateValueDto? DateValue { get; set; }

    [JsonProperty("multi_enum_values")]
    public IEnumerable<CustomFieldEnumValueDto>? MultiEnumValues { get; set; }


}
