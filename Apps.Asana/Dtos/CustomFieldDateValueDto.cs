using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class CustomFieldDateValueDto
{
    [JsonProperty("date")]
    public string? Date { get; set; }

    [JsonProperty("date_time")]
    public string? DateTime { get; set; }
}