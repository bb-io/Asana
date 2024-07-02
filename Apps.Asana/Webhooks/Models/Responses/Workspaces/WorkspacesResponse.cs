using Apps.Asana.Dtos;

namespace Apps.Asana.Webhooks.Models.Responses.Workspaces;

public class WorkspacesResponse
{
    public List<WorkspaceDto> Workspaces { get; set; } = new();
}