using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Dtos;

public class CustomFieldDto : AsanaEntity
{
    public string Type { get; set; }
    public string TextValue { get; set; }
    public IEnumerable<CustomFieldEnumValueDto>? EnumOptions { get; set; }
    public CustomFieldEnumValueDto? EnumValue { get; set; }
    public CustomFieldDateValueDto? DateValue { get; set; }
}