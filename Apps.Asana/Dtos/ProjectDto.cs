using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class ProjectDto : AsanaEntity
{
    public bool Archived { get; set; }
    public string Color { get; set; }
    public IEnumerable<AsanaEntity> Followers { get; set; }
    public IEnumerable<AsanaEntity> Members { get; set; }

    [Display("Permalink URL")] public string PermalinkUrl { get; set; }
    public bool Public { get; set; }
    public AsanaEntity Team { get; set; }
    public AsanaEntity Workspace { get; set; }
}