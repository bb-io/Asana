using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Projects.Responses;

public class GetProjectSectionsResponse
{
    public IEnumerable<AsanaEntity> Sections { get; set; }
}