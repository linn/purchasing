using Linn.Purchasing.IoC;
using Linn.Purchasing.Messaging.Host.Jobs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        {
            services.AddLog();
            services.AddCredentialsExtensions();
            services.AddServices();
            services.AddPersistence();
            services.AddSqsExtensions();
            services.AddRabbitConfiguration();
            services.AddMessageHandlers();
            services.AddHostedService<Listener>();
        })
    .Build();

await host.RunAsync();
