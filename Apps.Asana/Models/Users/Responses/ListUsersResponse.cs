using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Users.Responses;

public class ListUsersResponse
{
    public IEnumerable<AsanaEntity> Users { get; set; }
}