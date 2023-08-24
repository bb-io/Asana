using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Attachments.Requests;

public class AttachmentRequest
{
    [Display("Attachment ID")]
    public string AttachmentId { get; set; }
}