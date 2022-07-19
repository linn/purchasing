namespace Linn.Purchasing.Messaging.Dispatchers
{
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class EmailOrderBookMessageDispatcher : IMessageDispatcher<EmailOrderBookMessageResource>
    {
        private readonly ChannelConfiguration channelConfiguration;

        private readonly ILog logger;

        public EmailOrderBookMessageDispatcher(ChannelConfiguration channelConfiguration, ILog logger)
        {
            this.channelConfiguration = channelConfiguration;
            this.logger = logger;
        }

        public void Dispatch(EmailOrderBookMessageResource message)
        {
            var json = JsonConvert.SerializeObject(
                message,
                new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

            var body = Encoding.UTF8.GetBytes(json);

            this.channelConfiguration.ProducerChannel
                .BasicPublish(
                    this.channelConfiguration.Exchange,
                    EmailMrOrderBookMessage.RoutingKey,
                    false,
                    null,
                    body);

            this.logger.Info($"Published a message with routing key: {EmailMrOrderBookMessage.RoutingKey}");
        }
    }
}
