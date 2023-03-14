namespace Linn.Purchasing.Messaging.Host.Jobs
{
    using System.Threading;
    using System.Threading.Tasks;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Configuration;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Purchasing.Messaging.Messages;

    using Microsoft.Extensions.Hosting;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class Listener : BackgroundService
    {
        private readonly IModel channel;

        private readonly string queueName;

        private readonly ChannelConfiguration channelConfiguration;

        private readonly IServiceProvider serviceProvider;

        public Listener(
            // Handler<EmailMrOrderBookMessage> emailOrderBookMessageHandler,
            // Handler<EmailMonthlyForecastReportMessage> emailMonthlyForecastReportMessageHandler,
            // Handler<EmailPurchaseOrderReminderMessage> emailPurchaseOrderReminderMessageHandler,
            // EventingBasicConsumer consumer,
            ChannelConfiguration channelConfiguration,
            IServiceProvider serviceProvider)
            // ILog logger)
        {
            this.queueName = "purchasing";
            this.channelConfiguration = channelConfiguration;
            this.serviceProvider = serviceProvider;
            
            this.channel = this.channelConfiguration.ConsumerChannel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = this.serviceProvider.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILog>();
            var consumer = scope.ServiceProvider.GetRequiredService<EventingBasicConsumer>();
            var emailOrderBookMessageHandler =
                scope.ServiceProvider.GetRequiredService<Handler<EmailMrOrderBookMessage>>();
            var emailMonthlyForecastReportMessageHandler 
                = scope.ServiceProvider.GetRequiredService<Handler<EmailMonthlyForecastReportMessage>>();
            var emailPurchaseOrderReminderMessageHandler 
                = scope.ServiceProvider.GetRequiredService<Handler<EmailPurchaseOrderReminderMessage>>();

            logger.Info("Waiting for messages. To exit press CTRL+C");
            consumer.Received += (_, ea) =>
                {
                    // switch on message RoutingKey to decide which handler to use
                    // handlers process the message and return true if successful
                    // or log errors and return false if unsuccessful
                    bool success = ea.RoutingKey switch
                        {
                            EmailMrOrderBookMessage.RoutingKey => emailOrderBookMessageHandler.Handle(
                                new EmailMrOrderBookMessage(ea)),
                            EmailMonthlyForecastReportMessage.RoutingKey => emailMonthlyForecastReportMessageHandler.Handle(
                                new EmailMonthlyForecastReportMessage(ea)),
                            EmailPurchaseOrderReminderMessage.RoutingKey => emailPurchaseOrderReminderMessageHandler.Handle(
                                new EmailPurchaseOrderReminderMessage(ea)),
                            _ => false
                        };

                    if (success)
                    {
                        // acknowledge successfully handled messages
                        this.channelConfiguration.ConsumerChannel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        // reject problem messages
                        this.channelConfiguration.ConsumerChannel.BasicReject(ea.DeliveryTag, false);
                    }
                };
            this.channel.BasicConsume(queue: $"{this.queueName}.q", autoAck: false, consumer: consumer);
            await Task.Delay(1, stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            this.channelConfiguration.Connection.Dispose();
            return Task.CompletedTask;
        }
    }
}
