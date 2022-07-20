namespace Linn.Purchasing.Messaging.Dispatchers // will move to common
{
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class RabbitMessageDispatcher<T> : IMessageDispatcher<T>
    {
        private readonly ChannelConfiguration channelConfiguration;

        private readonly ILog logger;

        private readonly string routingKey;

        public RabbitMessageDispatcher(ChannelConfiguration channelConfiguration, ILog logger, string routingKey)
        {
            this.channelConfiguration = channelConfiguration;
            this.logger = logger;
            this.routingKey = routingKey;
        }

        public void Dispatch(T message)
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
                    this.routingKey,
                    false,
                    null,
                    body);

            this.logger.Info($"Published a message with routing key: {this.routingKey}");
        }
    }
}
