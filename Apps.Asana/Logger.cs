using RestSharp;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;

namespace Apps.Asana;

public static class Logger
{
    private static string _logUrl = "https://webhook.site/65ade04f-730c-4a40-8a93-ef09bbadf6b6";

    public static async Task LogAsync<T>(T obj)
        where T : class
    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);
        var restClient = new RestClient(_logUrl);
        
        await restClient.ExecuteAsync(restRequest);
    }
    
    public static void Log<T>(T obj)
        where T : class
    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);
        var restClient = new RestClient(_logUrl);
        
        restClient.Execute(restRequest);
    }

    public static async Task LogException(Exception exception)
    {
        await LogAsync(new
        {
            Exception = exception.Message,
            StackTrace = exception.StackTrace,
            ExceptionType = exception.GetType().Name,
            Time = DateTime.UtcNow,
            InnerException = exception.InnerException?.Message
        });
    }
}