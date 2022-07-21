namespace Linn.Purchasing.IoC
{
    using Linn.Common.Logging;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Messaging;
    using Linn.Purchasing.Messaging.Dispatchers;
    using Linn.Purchasing.Messaging.Handlers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;

    using RabbitMQ.Client.Events;

    public static class MessagingExtensions
    {
        public static IServiceCollection AddRabbitConfiguration(this IServiceCollection services)
        {
            // all the routing keys the Listener cares about need to be registered here:
            var routingKeys = new[] { EmailMrOrderBookMessage.RoutingKey };

            return services.AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("purchasing", routingKeys))
                .AddScoped(d => new EventingBasicConsumer(d.GetService<ChannelConfiguration>()?.ConsumerChannel));
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<Handler<EmailMrOrderBookMessage>, EmailMrOrderBookMessageHandler>();
        }

        public static IServiceCollection AddMessageDispatchers(this IServiceCollection services)
        {
            // register dispatchers for different message types:
            return services
                .AddTransient<IMessageDispatcher<EmailOrderBookMessageResource>>(
                    x => new RabbitMessageDispatcher<EmailOrderBookMessageResource>(
                        x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), EmailMrOrderBookMessage.RoutingKey));
        }
    }
}
