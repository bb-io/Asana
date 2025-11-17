using Apps.Asana.Dtos.Base;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Apps.Asana.Models.Attachments.Responses;

public class AttachmentResponse
{
    public string Gid { get; set; }
    
    public string Name { get; set; }

    [JsonProperty("download_url")]
    public string DownloadUrl { get; set; }

    public string PermanentUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public AsanaEntity? Parent { get; set; }
}