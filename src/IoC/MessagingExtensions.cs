namespace Linn.Purchasing.IoC
{
    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Configuration;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
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
            var routingKeys = new[]
                                  {
                                      EmailMrOrderBookMessage.RoutingKey, 
                                      EmailMonthlyForecastReportMessage.RoutingKey, 
                                      EmailPurchaseOrderReminderMessage.RoutingKey
                                  };
            return services.AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("purchasing", routingKeys, "purchasing"))
                .AddSingleton(d => new EventingBasicConsumer(d.GetService<ChannelConfiguration>()?.ConsumerChannel));
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<Handler<EmailMrOrderBookMessage>, EmailMrOrderBookMessageHandler>()
                .AddSingleton<Handler<EmailMonthlyForecastReportMessage>, EmailMonthlyForecastReportMessageHandler>()
                .AddSingleton<Handler<EmailPurchaseOrderReminderMessage>, EmailPurchaseOrderReminderMessageHandler>();
        }

        public static IServiceCollection AddMessageDispatchers(this IServiceCollection services)
        {
            // register dispatchers for different message types:
            return services
                .AddTransient<IMessageDispatcher<EmailOrderBookMessageResource>>(
                    x => new RabbitMessageDispatcher<EmailOrderBookMessageResource>(
                        x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), EmailMrOrderBookMessage.RoutingKey))
                .AddTransient<IMessageDispatcher<EmailMonthlyForecastReportMessageResource>>(
                    x => new RabbitMessageDispatcher<EmailMonthlyForecastReportMessageResource>(
                        x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), EmailMonthlyForecastReportMessage.RoutingKey))
                .AddTransient<IMessageDispatcher<EmailPurchaseOrderReminderMessageResource>>(
                    x => new RabbitMessageDispatcher<EmailPurchaseOrderReminderMessageResource>(
                        x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), EmailPurchaseOrderReminderMessage.RoutingKey));
        }
    }
}
