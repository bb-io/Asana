using Apps.Asana.Dtos.Base;

namespace Apps.Asana.Dtos;

public class WorkspaceDto : AsanaEntity
{
    public IEnumerable<string> EmailDomains { get; set; }
    public bool IsOrganization { get; set; }
}