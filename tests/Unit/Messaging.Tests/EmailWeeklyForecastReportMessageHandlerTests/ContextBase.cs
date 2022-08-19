namespace Linn.Purchasing.Messaging.Tests.EmailWeeklyForecastReportMessageHandlerTests
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Handlers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    using NSubstitute;

    using NUnit.Framework;

    using RabbitMQ.Client.Events;

    public class ContextBase
    {
        protected ISupplierAutoEmailsMailer Mailer { get; private set; }

        protected ILog Log { get; private set; }

        protected Handler<EmailWeeklyForecastReportMessage> Sut { get; private set; }

        protected EmailWeeklyForecastReportMessageResource Resource { get; private set; }

        protected EmailWeeklyForecastReportMessage Message { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Resource = new EmailWeeklyForecastReportMessageResource
            {
                                    ForSupplier = 123,
                                    ToAddress = "test@address.com"
                                };
            var json = JsonConvert.SerializeObject(this.Resource);

            var memory = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(json));

            var e = new BasicDeliverEventArgs
                        {
                            RoutingKey = EmailMrOrderBookMessage.RoutingKey,
                            Body = memory
                        };
            this.Message = new EmailWeeklyForecastReportMessage(e);

            this.Mailer = Substitute.For<ISupplierAutoEmailsMailer>();
            this.Log = Substitute.For<ILog>();
            this.Sut = new EmailWeeklyForecastReportMessageHandler(this.Log, this.Mailer);
        }
    }
}
