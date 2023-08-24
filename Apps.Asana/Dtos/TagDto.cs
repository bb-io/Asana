using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class TagDto : AsanaEntity
{
public string Color { get; set; }
    public string Notes { get; set; }

    [Display("Created at")] public DateTime CreatedAt { get; set; }

    public IEnumerable<AsanaEntity> Followers { get; set; }
    public AsanaEntity Workspace { get; set; }

    [Display("Permalink URL")] public string PermalinkUrl { get; set; }
}