namespace Linn.Purchasing.Messaging.Tests
{
    using Linn.Common.Messaging.RabbitMQ;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class MessagingContextBase
    {
        protected IMessageDispatcher MessageDispatcher { get; private set; }

        [SetUp]
        public void EstablishBaseContext()
        {
            this.MessageDispatcher = Substitute.For<IMessageDispatcher>();
        }
    }
}