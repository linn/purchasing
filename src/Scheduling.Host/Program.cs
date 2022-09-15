using Linn.Common.Scheduling;
using Linn.Purchasing.IoC;
using Linn.Purchasing.Scheduling.Host.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLog();
        services.AddCredentialsExtensions();
        services.AddSqsExtensions();
        services.AddServices();
        services.AddPersistence();
        services.AddRabbitConfiguration();
        services.AddMessageDispatchers();
        services.AddSingleton<CurrentTime>(() => DateTime.Now);
        services.AddHostedService<SupplierAutoEmailsScheduler>();
    })
    .Build();

await host.RunAsync();
