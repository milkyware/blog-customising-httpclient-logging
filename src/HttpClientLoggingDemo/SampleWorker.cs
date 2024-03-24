namespace HttpClientLoggingDemo
{
    public class SampleWorker(SampleService sampleService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await sampleService.PostRequestAsync();
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
