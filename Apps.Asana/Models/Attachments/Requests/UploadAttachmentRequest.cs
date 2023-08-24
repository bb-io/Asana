using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Attachments.Requests;

public class UploadAttachmentRequest
{
    public byte[] File { get; set; }
    
    [Display("File name")]
    public string FileName { get; set; }

    [Display("Parent ID")]
    public string ParentId { get; set; }
}