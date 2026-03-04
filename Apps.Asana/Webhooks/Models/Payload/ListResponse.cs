namespace Apps.Asana.Webhooks.Models.Payload
{
    public class ListResponse<T>
    {
        public List<T> Data { get; set; } = new();
    }
}
