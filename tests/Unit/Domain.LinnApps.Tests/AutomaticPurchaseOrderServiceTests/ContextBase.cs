namespace Linn.Purchasing.Domain.LinnApps.Tests.AutomaticPurchaseOrderServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected AutomaticPurchaseOrderService Sut { get; private set; }

        protected IPurchaseOrderAutoOrderPack PurchaseOrderAutoOrderPack { get; private set;  }

        protected IPurchaseOrdersPack PurchaseOrdersPack { get; private set; }

        protected ICurrencyPack CurrencyPack { get; private set; }
        
        protected IRepository<SigningLimit, int> SigningLimitRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseOrderAutoOrderPack = Substitute.For<IPurchaseOrderAutoOrderPack>();
            this.PurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.CurrencyPack = Substitute.For<ICurrencyPack>();
            this.SigningLimitRepository = Substitute.For<IRepository<SigningLimit, int>>();

            this.Sut = new AutomaticPurchaseOrderService(
                this.PurchaseOrderAutoOrderPack,
                this.PurchaseOrdersPack,
                this.CurrencyPack,
                this.SigningLimitRepository);
        }
    }
}
