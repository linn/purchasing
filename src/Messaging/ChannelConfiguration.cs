namespace Linn.Purchasing.Messaging
{
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Configuration;

    using RabbitMQ.Client;

    // this could move to Common
    public class ChannelConfiguration
    {
        public ChannelConfiguration(string queueName, string[] routingKeys)
        {
            this.Exchange = $"{queueName}.x";
            var x = Directory.GetCurrentDirectory();
            this.Connection = new ConnectionFactory
            {
                HostName = ConfigurationManager.Configuration["RABBIT_SERVER"],
                UserName = ConfigurationManager.Configuration["RABBIT_USERNAME"],
                Password = ConfigurationManager.Configuration["RABBIT_PASSWORD"],
                Port = int.Parse(ConfigurationManager.Configuration["RABBIT_PORT"])
            }.CreateConnection();

            var channels = new List<IModel> { this.Connection.CreateModel(), this.Connection.CreateModel() };
            foreach (var c in channels)
            {
                c.QueueDeclare(
                    queue: $"{queueName}.q",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: new Dictionary<string, object>
                                   {
                                       { "x-dead-letter-exchange", $"{queueName}.dlx" }
                                   });

                c.ExchangeDeclare(exchange: this.Exchange, type: ExchangeType.Direct);

                c.ExchangeDeclare(exchange: $"{queueName}.dlx", type: ExchangeType.Fanout);

                c.QueueDeclare(
                    queue: $"{queueName}.dlq",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                c.QueueBind(
                    queue: $"{queueName}.dlq",
                    exchange: $"{queueName}.dlx",
                    routingKey: string.Empty);

                foreach (var routingKey in routingKeys)
                {
                    c.QueueBind(
                        queue: $"{queueName}.q",
                        exchange: $"{queueName}.x",
                        routingKey: routingKey);
                }
            }

            this.ConsumerChannel = channels[0];
            this.ProducerChannel = channels[1];
        }

        public IModel ConsumerChannel { get; }

        public IModel ProducerChannel { get; }

        public IConnection Connection { get; }

        public string Exchange { get; }
    }
}
