using Linn.Purchasing.Scheduling.Host.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TestWorker>();
    })
    .Build();

await host.RunAsync();
