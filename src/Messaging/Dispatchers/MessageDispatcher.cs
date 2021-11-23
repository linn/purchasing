namespace Linn.Purchasing.Messaging.Dispatchers
{
    using System.Text;

    using Linn.Common.Messaging.RabbitMQ;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class MessageSender : IMessageSender
    {
        private readonly string contentType = "application/json";

        private readonly string routingKey = "purchasing.message";

        private readonly IMessageDispatcher messageDispatcher;

        public MessageSender(IMessageDispatcher messageDispatcher)
        {
            this.messageDispatcher = messageDispatcher;
        }

        public void SendMessage(string text)
        {
            var message = new MessageResource { Text = text };
            var json = JsonConvert.SerializeObject(
                message,
                new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

            var body = Encoding.UTF8.GetBytes(json);

            this.messageDispatcher.Dispatch(this.routingKey, body, this.contentType);
        }
    }
}
