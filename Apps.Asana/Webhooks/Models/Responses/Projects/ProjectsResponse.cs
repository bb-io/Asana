using Apps.Asana.Dtos;

namespace Apps.Asana.Webhooks.Models.Responses.Projects;

public class ProjectsResponse
{
    public List<ProjectDto> Projects { get; set; }
}