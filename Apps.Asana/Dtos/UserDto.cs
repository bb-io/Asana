using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Dtos;

public class UserDto : AsanaEntity
{
    public string Email { get; set; }

    public IEnumerable<AsanaEntity> Workspaces { get; set; }
}