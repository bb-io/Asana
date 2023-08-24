using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Tags.Responses;

public class ListTagsResponse
{
    public IEnumerable<AsanaEntity> Tags { get; set; }
}