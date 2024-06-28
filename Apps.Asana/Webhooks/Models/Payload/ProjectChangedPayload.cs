using Newtonsoft.Json;

namespace Apps.Asana.Webhooks.Models.Payload;

public class ProjectChangedPayload
{
    [JsonProperty("events")]
    public List<Event>? Events { get; set; }
}

public class User
{
    public string Gid { get; set; }
    public string ResourceType { get; set; }
}

public class Change
{
    public string Field { get; set; }
    public string Action { get; set; }
}

public class Resource
{
    public string Gid { get; set; }
    public string ResourceType { get; set; }
    public string ResourceSubtype { get; set; }
}

public class Event
{
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Action { get; set; }
    public object Parent { get; set; }
    public Change Change { get; set; }
    public Resource Resource { get; set; }
}