using Apps.Asana.Dtos.Base;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class AttachmentDto : AsanaEntity
{
    [Display("Download URL")]
    public string DownloadUrl { get; set; }

    [Display("Permanent URL")]
    public string PermanentUrl { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public AsanaEntity? Parent { get; set; }
}