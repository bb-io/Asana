using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Models.Attachments.Responses;

public class GetAttachmentsResponse
{
    public IEnumerable<AsanaEntity> Attachments { get; set; }
}