using Linn.Purchasing.Scheduling.Host.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TestWorker>();
    })
    .Build();

await host.RunAsync();
