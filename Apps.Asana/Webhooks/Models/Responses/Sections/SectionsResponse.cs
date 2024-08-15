using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Webhooks.Models.Responses.Sections;

public class SectionsResponse
{
    public List<AsanaEntity> Sections { get; set; } = new();
}