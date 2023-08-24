using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Sections.Response;

public class ListSectionsResponse
{
    public IEnumerable<AsanaEntity> Sections { get; set; }
}