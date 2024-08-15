using Apps.Asana.Dtos;

namespace Apps.Asana.Webhooks.Models.Responses.Tags;

public class TagsResponse
{
    public List<TagDto> Tags { get; set; } = new ();
}