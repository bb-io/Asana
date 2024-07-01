using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class StoryDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("html_text")]
    public string HtmlText { get; set; }

    [JsonProperty("is_pinned")]
    public bool IsPinned { get; set; }

    [JsonProperty("sticker_name")]
    public string StickerName { get; set; }

    [JsonProperty("created_by")]
    public AsanaUserDto CreatedBy { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("is_editable")]
    public bool IsEditable { get; set; }

    [JsonProperty("is_edited")]
    public bool IsEdited { get; set; }

    [JsonProperty("hearted")]
    public bool Hearted { get; set; }

    [JsonProperty("hearts")]
    public List<AsanaHeartDto> Hearts { get; set; }

    [JsonProperty("num_hearts")]
    public int NumHearts { get; set; }

    [JsonProperty("liked")]
    public bool Liked { get; set; }

    [JsonProperty("likes")]
    public List<AsanaLikeDto> Likes { get; set; }

    [JsonProperty("num_likes")]
    public int NumLikes { get; set; }

    [JsonProperty("previews")]
    public List<AsanaPreviewDto> Previews { get; set; }

    [JsonProperty("old_name")]
    public string OldName { get; set; }

    [JsonProperty("new_name")]
    public string NewName { get; set; }

    [JsonProperty("old_dates")]
    public AsanaDatesDto OldDates { get; set; }

    [JsonProperty("new_dates")]
    public AsanaDatesDto NewDates { get; set; }

    [JsonProperty("old_resource_subtype")]
    public string OldResourceSubtype { get; set; }

    [JsonProperty("new_resource_subtype")]
    public string NewResourceSubtype { get; set; }

    [JsonProperty("story")]
    public StoryDetailDto Story { get; set; }

    [JsonProperty("assignee")]
    public AsanaUserDto Assignee { get; set; }

    [JsonProperty("follower")]
    public AsanaUserDto Follower { get; set; }

    [JsonProperty("old_section")]
    public AsanaSectionDto OldSection { get; set; }

    [JsonProperty("new_section")]
    public AsanaSectionDto NewSection { get; set; }

    [JsonProperty("task")]
    public TaskDto Task { get; set; }

    [JsonProperty("project")]
    public AsanaProjectDto Project { get; set; }

    [JsonProperty("tag")]
    public AsanaTagDto Tag { get; set; }

    [JsonProperty("custom_field")]
    public AsanaCustomFieldDto CustomField { get; set; }

    [JsonProperty("old_text_value")]
    public string OldTextValue { get; set; }

    [JsonProperty("new_text_value")]
    public string NewTextValue { get; set; }

    [JsonProperty("old_number_value")]
    public double OldNumberValue { get; set; }

    [JsonProperty("new_number_value")]
    public double NewNumberValue { get; set; }

    [JsonProperty("old_enum_value")]
    public AsanaEnumValueDto OldEnumValue { get; set; }

    [JsonProperty("new_enum_value")]
    public AsanaEnumValueDto NewEnumValue { get; set; }

    [JsonProperty("old_date_value")]
    public AsanaDatesDto OldDateValue { get; set; }

    [JsonProperty("new_date_value")]
    public AsanaDatesDto NewDateValue { get; set; }

    [JsonProperty("old_people_value")]
    public List<AsanaUserDto> OldPeopleValue { get; set; }

    [JsonProperty("new_people_value")]
    public List<AsanaUserDto> NewPeopleValue { get; set; }

    [JsonProperty("old_multi_enum_values")]
    public List<AsanaEnumValueDto> OldMultiEnumValues { get; set; }

    [JsonProperty("new_multi_enum_values")]
    public List<AsanaEnumValueDto> NewMultiEnumValues { get; set; }

    [JsonProperty("new_approval_status")]
    public string NewApprovalStatus { get; set; }

    [JsonProperty("old_approval_status")]
    public string OldApprovalStatus { get; set; }

    [JsonProperty("duplicate_of")]
    public TaskDto DuplicateOf { get; set; }

    [JsonProperty("duplicated_from")]
    public TaskDto DuplicatedFrom { get; set; }

    [JsonProperty("dependency")]
    public TaskDto Dependency { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("target")]
    public TaskDto Target { get; set; }
}

public class AsanaHeartDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("user")]
    public AsanaUserDto User { get; set; }
}

public class AsanaPreviewDto
{
    [JsonProperty("fallback")]
    public string Fallback { get; set; }

    [JsonProperty("footer")]
    public string Footer { get; set; }

    [JsonProperty("header")]
    public string Header { get; set; }

    [JsonProperty("header_link")]
    public string HeaderLink { get; set; }

    [JsonProperty("html_text")]
    public string HtmlText { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("title_link")]
    public string TitleLink { get; set; }
}

public class AsanaDatesDto
{
    [JsonProperty("start_on")]
    public DateTime? StartOn { get; set; }

    [JsonProperty("due_at")]
    public DateTime? DueAt { get; set; }

    [JsonProperty("due_on")]
    public DateTime? DueOn { get; set; }
}

public class StoryDetailDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("created_by")]
    public AsanaUserDto CreatedBy { get; set; }

    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}

public class AsanaSectionDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaTagDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaCustomFieldDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("resource_subtype")]
    public string ResourceSubtype { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("enum_options")]
    public List<AsanaEnumOptionDto> EnumOptions { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("representation_type")]
    public string RepresentationType { get; set; }

    [JsonProperty("id_prefix")]
    public string IdPrefix { get; set; }

    [JsonProperty("is_formula_field")]
    public bool IsFormulaField { get; set; }

    [JsonProperty("date_value")]
    public AsanaDateValueDto DateValue { get; set; }

    [JsonProperty("enum_value")]
    public AsanaEnumValueDto EnumValue { get; set; }

    [JsonProperty("multi_enum_values")]
    public List<AsanaEnumValueDto> MultiEnumValues { get; set; }

    [JsonProperty("number_value")]
    public double NumberValue { get; set; }

    [JsonProperty("text_value")]
    public string TextValue { get; set; }

    [JsonProperty("display_value")]
    public string DisplayValue { get; set; }
}

public class AsanaEnumOptionDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }
}

public class AsanaEnumValueDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }
}

public class AsanaDateValueDto
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("date_time")]
    public DateTime DateTime { get; set; }
}
