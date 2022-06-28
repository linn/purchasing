namespace Linn.Purchasing.Scheduling.Host.Services
{
    public class TestWorker : BackgroundService
    {
        private readonly ILogger<TestWorker> logger;

        public TestWorker(ILogger<TestWorker> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTimeOffset.Now.Hour == 10 && DateTimeOffset.Now.Minute == 5)
                {
                    this.logger.LogInformation("its five past ten", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}