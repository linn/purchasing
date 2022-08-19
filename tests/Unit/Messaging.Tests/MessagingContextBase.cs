namespace Linn.Purchasing.Messaging.Tests
{
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class MessagingContextBase
    {
        [SetUp]
        public void EstablishBaseContext()
        {
        }
    }
}
