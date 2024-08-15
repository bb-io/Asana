using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Attachments.Responses;

public class AttachmentResponse
{
    public string Gid { get; set; }
    
    public string Name { get; set; }
    
    public string DownloadUrl { get; set; }

    public string PermanentUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public AsanaEntity? Parent { get; set; }
}