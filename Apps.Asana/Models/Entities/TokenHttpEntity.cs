using System.Net;

namespace Apps.Asana.Models.Entities;

public record TokenHttpEntity(HttpStatusCode StatusCode, string? ContentType, string? Content)
{
    public bool IsSuccessStatusCode => (int)StatusCode is >= 200 and <= 299;

    public bool IsHtmlContent =>
        ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true
        || Content?.TrimStart().StartsWith('<') == true;

    public string StatusDescription => $"{(int)StatusCode} ({StatusCode})";
}
