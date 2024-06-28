using Apps.Asana.Dtos;

namespace Apps.Asana.Webhooks.Models.Responses;

public class ProjectsResponse
{
    public List<ProjectDto> Projects { get; set; }
}