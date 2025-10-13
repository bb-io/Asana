using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos.Base;

public class AsanaEntity
{
    [Display("Task ID")]
    public string Gid { get; set; }
    
    public string Name { get; set; }
}