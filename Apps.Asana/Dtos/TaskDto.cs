using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class TaskDto : AsanaEntity
{
    public string Notes { get; set; }
    public bool Completed { get; set; }
    public AsanaEntity Workspace { get; set; }
    public AsanaEntity Assignee { get; set; }
    public IEnumerable<AsanaEntity> Followers { get; set; }
    public IEnumerable<AsanaEntity> Projects { get; set; }

    [Display("Created at")] public DateTime CreatedAt { get; set; }
    [Display("Permalink URL")] public string PermalinkUrl { get; set; }
}