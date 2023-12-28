using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Asana.Models.Attachments.Requests;

public class UploadAttachmentRequest
{
    public FileReference File { get; set; }
    
    [Display("File name")]
    public string? FileName { get; set; }

    [Display("Parent ID")]
    public string ParentId { get; set; }
}