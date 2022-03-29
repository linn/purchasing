namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderReqService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IPurchaseOrderReqsPack MockPurchaseOrderReqsPack { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockPurchaseOrderReqsPack = Substitute.For<IPurchaseOrderReqsPack>();

            this.Sut = new PurchaseOrderReqService(
                this.MockAuthService,
                MockPurchaseOrderReqsPack);
        }
    }
}
