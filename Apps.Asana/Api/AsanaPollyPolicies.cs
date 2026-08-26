using System.Globalization;
using System.Net;
using Polly;
using Polly.Retry;
using RestSharp;

namespace Apps.Asana.Api;

public static class AsanaPollyPolicies
{
    internal const int DefaultRetryCount = 5;
    private const double MinimumFallbackDelaySeconds = 5;
    private const double MaximumFallbackDelaySeconds = 45;

    public static ResiliencePipeline<RestResponse> CreateRateLimitPipeline(int retryCount = DefaultRetryCount)
    {
        var options = new RetryStrategyOptions<RestResponse>
        {
            MaxRetryAttempts = retryCount,
            ShouldHandle = new PredicateBuilder<RestResponse>()
                .HandleResult(ShouldRetry)
                .Handle<HttpRequestException>(exception =>
                    exception.StatusCode == HttpStatusCode.TooManyRequests),
            DelayGenerator = args => new ValueTask<TimeSpan?>(
                GetDelay(args.Outcome.Result))
        };

        return new ResiliencePipelineBuilder<RestResponse>()
            .AddRetry(options)
            .Build();
    }

    internal static bool ShouldRetry(RestResponse response)
        => response.StatusCode == HttpStatusCode.TooManyRequests;

    internal static bool TryGetServerDelay(RestResponse response, out TimeSpan delay)
    {
        if (TryGetHeaderValue(response, "Retry-After", out var retryAfter))
        {
            if (TryParseNonNegativeNumber(retryAfter, out var seconds))
            {
                delay = TimeSpan.FromSeconds(seconds);
                return true;
            }

            if (DateTimeOffset.TryParse(retryAfter, CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal, out var retryAt))
            {
                delay = retryAt - DateTimeOffset.UtcNow;
                if (delay < TimeSpan.Zero)
                {
                    delay = TimeSpan.Zero;
                }

                return true;
            }
        }

        delay = default;
        return false;
    }

    internal static TimeSpan GetDelay(RestResponse? response)
    {
        if (response is not null && TryGetServerDelay(response, out var serverDelay))
        {
            return serverDelay;
        }

        var delaySeconds = Random.Shared.NextDouble() *
            (MaximumFallbackDelaySeconds - MinimumFallbackDelaySeconds) +
            MinimumFallbackDelaySeconds;

        return TimeSpan.FromSeconds(delaySeconds);
    }

    private static bool TryGetHeaderValue(RestResponse response, string name, out string value)
    {
        value = response.Headers?
            .FirstOrDefault(header => header.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true)
            ?.Value?.ToString() ?? string.Empty;

        return !string.IsNullOrWhiteSpace(value);
    }

    private static bool TryParseNonNegativeNumber(string value, out double number) =>
        double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number) &&
        double.IsFinite(number) &&
        number >= 0;
}
