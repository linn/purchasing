using Linn.Purchasing.IoC;
using Linn.Purchasing.Scheduling.Host.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLog();
        services.AddCredentialsExtensions();
        services.AddSqsExtensions();
        services.AddMessagingServices();
        services.AddHostedService<EmailOrderBookScheduler>();
    })
    .Build();

await host.RunAsync();
