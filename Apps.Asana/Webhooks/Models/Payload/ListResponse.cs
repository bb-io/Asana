using Newtonsoft.Json;

namespace Apps.Asana.Webhooks.Models.Payload
{
    public class ListResponse<T>
    {
        [JsonProperty("data")]
        public List<T> Data { get; set; } = new();
    }
}
