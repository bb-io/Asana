using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Attachments.Requests;

public class ListAttachmentsRequest
{
    [Display("Object ID")]
    public string ObjectId { get; set; }
}