namespace Linn.Purchasing.Messaging.Host
{
    using System;
    using System.Threading;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ;
    using Linn.Common.Messaging.RabbitMQ.Unicast;
    using Linn.Purchasing.Messaging.Handlers;

    public class Listener
    {
        private readonly IReceiver receiver;
        private readonly DedupingMessageConsumer consumer;
        private readonly ILog logger;

        public Listener(IReceiver receiver, ILog logger)
        {
            this.logger = logger;
            this.receiver = receiver;
            this.consumer = new DedupingMessageConsumer(new MessageConsumer(this.receiver), this.receiver);

            this.logger.Info("Started purchasing-listener");

            this.consumer.For("purchasing.test")
                .OnConsumed(m =>
                    {
                        var handler = new TestHandler(logger);
                        return handler.Execute(m);
                    })
                .OnRejected(this.LogRejection);
        }

        public void Listen()
        {
            try
            {
                var message = this.receiver.Receive(5000);
                this.consumer.Consume(message);
            }
            catch (Exception e)
            {
                this.logger.Error("Exception thrown by message handler: " + e.Message, e);
                Thread.Sleep(1000);
            }
        }

        private void LogRejection(IReceivedMessage message)
        {
            Console.WriteLine($"The message with Id {message.MessageId} was rejected.");
            this.logger.Error($"The message with Id {message.MessageId} was rejected.");
        }
    }
}
