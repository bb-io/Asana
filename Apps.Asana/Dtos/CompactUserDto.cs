using Newtonsoft.Json;

namespace Apps.Asana.Dtos
{
    public class CompactUserDto
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
