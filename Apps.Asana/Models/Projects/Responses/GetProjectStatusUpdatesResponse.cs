using Apps.Asana.Dtos;

namespace Apps.Asana.Models.Projects.Responses;

public class GetProjectStatusUpdatesResponse
{
    public IEnumerable<ProjectStatusUpdateDto> Updates { get; set; }
}