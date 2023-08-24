using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Dtos;

public class ProjectStatusDto
{
    [Display("ID")]
    public string Gid { get; set; }

    public string Color { get; set; }

    public string Text { get; set; }

    public string Title { get; set; }
}