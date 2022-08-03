using Linn.Purchasing.IoC;
using Linn.Purchasing.Scheduling.Host.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLog();
        services.AddPersistence();
        services.AddCredentialsExtensions();
        services.AddSqsExtensions();
        services.AddServices();
        services.AddRabbitConfiguration();
        services.AddMessageDispatchers();
        services.AddHostedService<EmailOrderBookScheduler>();
    })
    .Build();

await host.RunAsync();
