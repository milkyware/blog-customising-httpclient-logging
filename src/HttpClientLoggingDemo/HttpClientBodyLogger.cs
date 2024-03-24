using Microsoft.Extensions.Http.Logging;

namespace HttpClientLoggingDemo
{
    public class HttpClientBodyLogger(ILogger logger) : IHttpClientAsyncLogger
    {
        public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed) => throw new NotImplementedException();

        public ValueTask LogRequestFailedAsync(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed, CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

        public object? LogRequestStart(HttpRequestMessage request) => throw new NotImplementedException();

        public async ValueTask<object?> LogRequestStartAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            logger.LogTrace($"Request Body:{Environment.NewLine}{await request.Content.ReadAsStringAsync()}");
            return null;
        }

        public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed) => throw new NotImplementedException();

        public async ValueTask LogRequestStopAsync(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed, CancellationToken cancellationToken = default)
        {
            logger.LogTrace($"Response Body:{Environment.NewLine}{await response.Content.ReadAsStringAsync()}");
        }
    }
}
