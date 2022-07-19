using Linn.Purchasing.IoC;
using Linn.Purchasing.Messaging.Host;
using Linn.Purchasing.Messaging.Host.Jobs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        {
            services.AddLog();
            services.AddCredentialsExtensions();
            services.AddSqsExtensions();
            services.AddMessagingServices();
            services.AddHostedService<Listener>();
        })
    .Build();

await host.RunAsync();