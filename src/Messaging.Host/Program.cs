namespace Linn.Purchasing.Messaging.Host
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Configuration;
    using Linn.Common.Messaging.RabbitMQ.Unicast;
    using Linn.Purchasing.Messaging.Handlers;

    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddTransient<IRabbitConfiguration, RabbitConfiguration>();
                services.AddTransient<TestHandler, TestHandler>();

                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var builder = new ConnectionBuilder(serviceProvider.GetService<IRabbitConfiguration>());
                services.AddTransient(
                    _ => MessagingFactory.CreateConnector(builder, new Connector.RetryInfinitely(100, i => { })));
                
                var receiver = MessagingFactory.CreateReceiver();
                receiver.Connector = MessagingFactory.CreateConnector(builder);
                receiver.Identity = "purchasing.q";
                receiver.Exclusive = false;
                receiver.Setup += channel =>
                    {
                        channel.BasicQos(0, 1, false);
                        channel.QueueDeclare(
                            "purchasing.q",
                            true,
                            false,
                            false,
                            new Dictionary<string, object>
                                {
                                    { "x-ha-policy", "all" },
                                    { "x-dead-letter-routing-key", "purchasing.dlq" },
                                    { "x-dead-letter-exchange", "purchasing.dlx" }
                                });
                    };
                receiver.Init();
                var listener = new Listener(receiver, new ConsoleLog());

                while (true)
                {
                    listener.Listen();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message} {e.InnerException?.Message}");
                Environment.Exit(1);
            }
        }
    }
}
