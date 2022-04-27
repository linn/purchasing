namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IDatabaseService MockDatabaseService { get; private set; }

        protected IPurchaseLedgerPack PurchaseLedgerPack { get; private set; }



        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockDatabaseService = Substitute.For<IDatabaseService>();
            this.PurchaseLedgerPack = Substitute.For<IPurchaseLedgerPack>();

            this.Sut = new PurchaseOrderService(
                this.MockAuthService, this.PurchaseLedgerPack, this.MockDatabaseService);
        }
    }
}
