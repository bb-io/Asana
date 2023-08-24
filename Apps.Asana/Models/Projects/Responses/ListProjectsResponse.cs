using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Projects.Responses;

public class ListProjectsResponse
{
    public IEnumerable<AsanaEntity> Projects { get; set; }
}