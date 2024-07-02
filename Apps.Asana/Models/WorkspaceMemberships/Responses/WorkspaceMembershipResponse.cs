using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.WorkspaceMemberships.Responses;

public class WorkspaceMembershipResponse
{
    [Display("Workspace membership ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("User name")]
    public string UserName { get; set; }

    [Display("Workspace name")]
    public string WorkspaceName { get; set; }

    [Display("User task list name")]
    public string UserTaskListName { get; set; }

    [Display("Task list owner name")]
    public string TaskListOwnerName { get; set; }

    [Display("Active status")]
    public bool IsActive { get; set; }

    [Display("Admin status")]
    public bool IsAdmin { get; set; }

    [Display("Guest status")]
    public bool IsGuest { get; set; }

    [Display("Vacation start date")]
    public DateTime? VacationStartDate { get; set; }

    [Display("Vacation end date")]
    public DateTime? VacationEndDate { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public WorkspaceMembershipResponse(WorkspaceMembershipDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        UserName = dto.User?.Name;
        WorkspaceName = dto.Workspace?.Name;
        UserTaskListName = dto.UserTaskList?.Name;
        TaskListOwnerName = dto.UserTaskList?.Owner?.Name;
        IsActive = dto.IsActive;
        IsAdmin = dto.IsAdmin;
        IsGuest = dto.IsGuest;
        VacationStartDate = dto.VacationDates?.StartOn;
        VacationEndDate = dto.VacationDates?.EndOn;
        CreatedAt = dto.CreatedAt;
    }
}