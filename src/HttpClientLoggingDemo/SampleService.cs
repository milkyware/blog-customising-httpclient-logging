namespace HttpClientLoggingDemo
{
    public class SampleService(ILogger<SampleService> logger, HttpClient httpClient)
    {
        public async Task PostRequestAsync()
        {
            logger.LogInformation("Executing {operation}", nameof(PostRequestAsync));
            await httpClient.PostAsJsonAsync("https://localhost:7139/sample", new SampleRequest() { Value = Guid.NewGuid().ToString() });
            logger.LogInformation("Received {operation} response", nameof(PostRequestAsync));
        }
    }
}
