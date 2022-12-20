namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierKitServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomDetailViewRepository BomDetailViewRepository { get; private set; }

        protected ISupplierKitService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomDetailViewRepository = Substitute.For<IBomDetailViewRepository>();
            this.Sut = new SupplierKitService(this.BomDetailViewRepository);
        }
    }
}
