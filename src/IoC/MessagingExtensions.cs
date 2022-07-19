namespace Linn.Purchasing.IoC
{
    
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
        public static IServiceCollection AddMessagingServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("play", new[] { "type-a" }))
                .AddScoped(d => new EventingBasicConsumer(
                    d.GetService<ChannelConfiguration>()?.ConsumerChannel))

                // add handlers for different message types
                .AddScoped<Handler<EmailMrOrderBookMessage>, EmailMrOrderBookMessageHandler>()

                // add dispatchers for different message types
                .AddTransient<IMessageDispatcher<EmailOrderBookMessageResource>, EmailOrderBookMessageDispatcher>();
        }
    }
}
