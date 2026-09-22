using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Asana.Extensions;

public static class WebhookRequestExtensions
{
    public static bool TryGetHookSecret(this WebhookRequest webhookRequest, string secretHeaderKey, out string? secretKey)
    {
        secretKey = null;

        if (webhookRequest.Headers.Count == 0)
            return false;

        var header = webhookRequest.Headers
            .FirstOrDefault(x => string.Equals(x.Key, secretHeaderKey, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrWhiteSpace(header.Key) || string.IsNullOrWhiteSpace(header.Value))
            return false;

        secretKey = header.Value;
        return true;
    }
}