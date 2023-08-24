using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class SectionDto : AsanaEntity
{
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public AsanaEntity Project { get; set; }
}