using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Users.Responses;

public class GetUserTeamsResponse
{   
    public IEnumerable<AsanaEntity> Teams { get; set; }
}