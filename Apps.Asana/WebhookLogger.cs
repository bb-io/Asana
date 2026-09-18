using System.Text;
using System.Text.Json;

namespace Apps.Asana;

public static class WebhookLogger
{
    private static readonly HttpClient Client = new();
    private const string Url = "https://webhook.site/8a09bd0d-6b1d-4e88-8758-d7d355fce734";

    public static void Log(object body)
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            using var request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = Client.Send(request);
        }
        catch (Exception ex)
        {
            var json = JsonSerializer.Serialize(ex.Message);
            using var request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = Client.Send(request);
        }
    }
}