namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IAuthorisationService MockAuthService { get; private set; }

        protected IPurchaseOrderReqsPack MockPurchaseOrderReqsPack { get; private set; }

        protected IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey> MockReqsStateChangeRepository
        {
            get;
            private set;
        }

        protected IPurchaseOrderReqService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockPurchaseOrderReqsPack = Substitute.For<IPurchaseOrderReqsPack>();
            this.MockReqsStateChangeRepository =
                Substitute.For<IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>>();

            this.Sut = new PurchaseOrderReqService(
                this.MockAuthService,
                this.MockPurchaseOrderReqsPack,
                this.MockReqsStateChangeRepository);
        }
    }
}
