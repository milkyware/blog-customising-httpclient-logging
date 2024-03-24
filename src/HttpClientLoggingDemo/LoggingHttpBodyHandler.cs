namespace HttpClientLoggingDemo
{
    public class LoggingHttpBodyHandler(ILogger logger) : DelegatingHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
            => SendCoreAsync(request, false, cancellationToken).GetAwaiter().GetResult();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => SendCoreAsync(request, true, cancellationToken);

        private async Task<HttpResponseMessage> SendCoreAsync(HttpRequestMessage request, bool useAsync, CancellationToken cancellationToken)
        {
            if (request.Content is not null)
            {
                logger.LogTrace($"Request Body:{Environment.NewLine}{await request.Content.ReadAsStringAsync()}");
            }

            HttpResponseMessage response = useAsync
                ? await base.SendAsync(request, cancellationToken).ConfigureAwait(false)
                : base.Send(request, cancellationToken);

            if (response.Content is not null)
            {
                var content = await response.Content.ReadAsStringAsync();
                logger.LogTrace($"Response Body:{Environment.NewLine}{await response.Content.ReadAsStringAsync()}");
            }

            return response;
        }
    }
}
