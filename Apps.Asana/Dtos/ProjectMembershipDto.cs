using Newtonsoft.Json;

namespace Apps.Asana.Dtos;

public class ProjectMembershipDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("parent")]
    public AsanaParentDto Parent { get; set; }

    [JsonProperty("member")]
    public AsanaMemberDto Member { get; set; }

    [JsonProperty("access_level")]
    public string AccessLevel { get; set; }

    [JsonProperty("user")]
    public AsanaUserDto User { get; set; }

    [JsonProperty("project")]
    public AsanaProjectDto Project { get; set; }

    [JsonProperty("write_access")]
    public string WriteAccess { get; set; }
}

public class AsanaParentDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaMemberDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class AsanaProjectDto
{
    [JsonProperty("gid")]
    public string Gid { get; set; }

    [JsonProperty("resource_type")]
    public string ResourceType { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}