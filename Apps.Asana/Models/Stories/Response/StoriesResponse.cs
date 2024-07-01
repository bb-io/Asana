using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Stories.Response;

public class StoriesResponse
{
    [Display("Stories")]
    public List<StoryResponse> Stories { get; set; } = new();
}