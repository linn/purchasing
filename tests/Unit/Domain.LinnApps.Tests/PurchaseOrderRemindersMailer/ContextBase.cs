namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderRemindersMailer
{
    using Linn.Common.Email;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IEmailService MockEmailService { get; private set; }

        protected IPurchaseOrderDeliveryRepository MockRepository { get; private set; }

        protected IPurchaseOrderRemindersMailer Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockEmailService = Substitute.For<IEmailService>();
            this.MockRepository = Substitute.For<IPurchaseOrderDeliveryRepository>();
            this.Sut = new PurchaseOrderRemindersMailer(this.MockEmailService, this.MockRepository);
        }
    }
}
