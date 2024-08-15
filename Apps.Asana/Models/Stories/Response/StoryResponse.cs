using Apps.Asana.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Asana.Models.Stories.Response;

public class StoryResponse
{
    [Display("Story ID")]
    public string Gid { get; set; }

    [Display("Resource type")]
    public string ResourceType { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Resource subtype")]
    public string ResourceSubtype { get; set; }

    [Display("Text")]
    public string Text { get; set; }

    [Display("HTML text")]
    public string HtmlText { get; set; }

    [Display("Created by")]
    public string CreatedByName { get; set; }

    [Display("Editable")]
    public bool IsEditable { get; set; }

    [Display("Hearted")]
    public bool Hearted { get; set; }

    [Display("Number of hearts")]
    public int NumHearts { get; set; }

    [Display("Liked")]
    public bool Liked { get; set; }

    [Display("Number of likes")]
    public int NumLikes { get; set; }

    [Display("Old name")]
    public string OldName { get; set; }

    [Display("New name")]
    public string NewName { get; set; }

    [Display("Old due date")]
    public DateTime? OldDueDate { get; set; }

    [Display("New due date")]
    public DateTime? NewDueDate { get; set; }

    [Display("Assignee")]
    public string AssigneeName { get; set; }

    [Display("Follower")]
    public string FollowerName { get; set; }

    [Display("Project")]
    public string ProjectName { get; set; }

    [Display("Task")]
    public string TaskName { get; set; }

    [Display("Tag")]
    public string TagName { get; set; }

    [Display("Custom field")]
    public string CustomFieldName { get; set; }

    public StoryResponse(StoryDto dto)
    {
        Gid = dto.Gid;
        ResourceType = dto.ResourceType;
        CreatedAt = dto.CreatedAt;
        ResourceSubtype = dto.ResourceSubtype;
        Text = dto.Text;
        HtmlText = dto.HtmlText;
        CreatedByName = dto.CreatedBy?.Name;
        IsEditable = dto.IsEditable;
        Hearted = dto.Hearted;
        NumHearts = dto.NumHearts;
        Liked = dto.Liked;
        NumLikes = dto.NumLikes;
        OldName = dto.OldName;
        NewName = dto.NewName;
        OldDueDate = dto.OldDates?.DueOn;
        NewDueDate = dto.NewDates?.DueOn;
        AssigneeName = dto.Assignee?.Name;
        FollowerName = dto.Follower?.Name;
        ProjectName = dto.Project?.Name;
        TaskName = dto.Task?.Name;
        TagName = dto.Tag?.Name;
        CustomFieldName = dto.CustomField?.Name;
    }
}