using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Goals;

public class GoalResponse
{
    [Display("Task ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("Task name")]
    public string Name { get; set; }

    [Display("HTML notes")]
    public string HtmlNotes { get; set; }

    [Display("Notes")]
    public string Notes { get; set; }

    [Display("Due date")]
    public DateTime? DueOn { get; set; }

    [Display("Start date")]
    public DateTime? StartOn { get; set; }

    [Display("Workspace level")]
    public bool IsWorkspaceLevel { get; set; }

    [Display("Liked")]
    public bool Liked { get; set; }

    [Display("Number of likes")]
    public int NumLikes { get; set; }

    [Display("Team")]
    public string TeamName { get; set; }

    [Display("Workspace")]
    public string WorkspaceName { get; set; }

    [Display("Owner")]
    public string OwnerName { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Time period")]
    public string TimePeriodDisplayName { get; set; }

    [Display("Metric precision")]
    public int MetricPrecision { get; set; }

    [Display("Metric unit")]
    public string MetricUnit { get; set; }

    [Display("Initial number value")]
    public double InitialNumberValue { get; set; }

    [Display("Target number value")]
    public double TargetNumberValue { get; set; }

    [Display("Current number value")]
    public double CurrentNumberValue { get; set; }

    [Display("Progress source")]
    public string ProgressSource { get; set; }

    [Display("Custom weight")]
    public bool IsCustomWeight { get; set; }

    public GoalResponse(GoalDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        Name = dto.Name;
        HtmlNotes = dto.HtmlNotes;
        Notes = dto.Notes;
        DueOn = dto.DueOn;
        StartOn = dto.StartOn;
        IsWorkspaceLevel = dto.IsWorkspaceLevel;
        Liked = dto.Liked;
        NumLikes = dto.NumLikes;
        TeamName = dto.Team?.Name;
        WorkspaceName = dto.Workspace?.Name;
        OwnerName = dto.Owner?.Name;
        Status = dto.Status;
        TimePeriodDisplayName = dto.TimePeriod?.DisplayName;
        MetricPrecision = dto.Metric?.Precision ?? 0;
        MetricUnit = dto.Metric?.Unit;
        InitialNumberValue = dto.Metric?.InitialNumberValue ?? 0;
        TargetNumberValue = dto.Metric?.TargetNumberValue ?? 0;
        CurrentNumberValue = dto.Metric?.CurrentNumberValue ?? 0;
        ProgressSource = dto.Metric?.ProgressSource;
        IsCustomWeight = dto.Metric?.IsCustomWeight ?? false;
    }
}