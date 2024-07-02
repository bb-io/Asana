using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.ProjectMemberships.Responses;

public class ProjectMembershipResponse
{
    [Display("Project membership ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("Parent name")]
    public string ParentName { get; set; }

    [Display("Member name")]
    public string MemberName { get; set; }

    [Display("Access level")]
    public string AccessLevel { get; set; }

    [Display("User name")]
    public string UserName { get; set; }

    [Display("Project name")]
    public string ProjectName { get; set; }

    [Display("Write access")]
    public string WriteAccess { get; set; }

    public ProjectMembershipResponse(ProjectMembershipDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        ParentName = dto.Parent?.Name;
        MemberName = dto.Member?.Name;
        AccessLevel = dto.AccessLevel;
        UserName = dto.User?.Name;
        ProjectName = dto.Project?.Name;
        WriteAccess = dto.WriteAccess;
    }
}