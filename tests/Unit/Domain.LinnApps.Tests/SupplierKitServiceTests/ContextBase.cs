namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierKitServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;
    using NSubstitute.Exceptions;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomDetailRepository bomDetailRepository { get; private set; }

        protected ISupplierKitService Sut { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.bomDetailRepository = Substitute.For<IBomDetailRepository>();
            this.Sut = new SupplierKitService(this.bomDetailRepository);
        }
    }
}
