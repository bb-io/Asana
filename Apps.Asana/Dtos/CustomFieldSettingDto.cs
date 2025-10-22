
using Apps.Asana.Dtos.Base;
using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class CustomFieldSettingDto
{
    public string Gid { get; set; }

    [JsonProperty("custom_field")]
    public Custom_FieldDto CustomField { get; set; }

    [JsonProperty("project")]
    public AsanaEntity Project { get; set; }
}

public class Custom_FieldDto : AsanaEntity
{
    public string Type { get; set; }

    [JsonProperty("enum_options")]
    public IEnumerable<CustomFieldEnumValueDto>? EnumOptions { get; set; }

    [JsonProperty("multi_enum_values")]
    public IEnumerable<CustomFieldEnumValueDto>? MultiEnumValues { get; set; }

    [JsonProperty("enum_value")]
    public CustomFieldEnumValueDto? EnumValue { get; set; }

    [JsonProperty("date_value")]
    public CustomFieldDateValueDto? DateValue { get; set; }
}
