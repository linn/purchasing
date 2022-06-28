namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Unicast;

    using Newtonsoft.Json;

    public class TestHandler
    {
        private readonly ILog log;


        public TestHandler(ILog log)
        {
            this.log = log;
        }

        public bool Execute(IReceivedMessage message)
        {
            var content = Encoding.UTF8.GetString(message.Body);
            return false;
        }
    }
}
