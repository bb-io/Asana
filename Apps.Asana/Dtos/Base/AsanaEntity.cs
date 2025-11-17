using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos.Base;

public class AsanaEntity
{
    [Display("ID")]
    public string Gid { get; set; }
    
    public string Name { get; set; }
}