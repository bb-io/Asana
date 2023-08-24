using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Workspaces.Responses;

public class ListWorkspacesResponse
{
    public IEnumerable<AsanaEntity> Workspaces { get; set; }
}