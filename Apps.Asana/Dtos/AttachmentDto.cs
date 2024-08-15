using Apps.Asana.Dtos.Base;
using Apps.Asana.Models.Attachments.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Asana.Dtos;

public class AttachmentDto : AsanaEntity
{
    public FileReference File { get; set; }

    [Display("Permanent URL")] public string PermanentUrl { get; set; }

    [Display("Created at")] public DateTime CreatedAt { get; set; }

    public AsanaEntity? Parent { get; set; }

    public AttachmentDto()
    {
        
    }

    public AttachmentDto(AttachmentResponse response)
    {
        Gid = response.Gid;
        Name = response.Name;
        PermanentUrl = response.PermanentUrl;
        CreatedAt = response.CreatedAt;
        Parent = response.Parent;
    }
}