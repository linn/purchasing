namespace Linn.Purchasing.Facade.Tests.AutomaticPurchaseOrderFacadeServiceTests
{
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected AutomaticPurchaseOrderFacadeService Sut { get; private set; }

        protected IAutomaticPurchaseOrderService AutomaticPurchaseOrderService { get; private set; }

        protected IRepository<AutomaticPurchaseOrder, int> AutomaticPurchaseOrderRepository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IBuilder<AutomaticPurchaseOrder> AutomaticPurchaseOrderBuilder { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AutomaticPurchaseOrderRepository = Substitute.For<IRepository<AutomaticPurchaseOrder, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.AutomaticPurchaseOrderService = Substitute.For<IAutomaticPurchaseOrderService>();
            this.AutomaticPurchaseOrderBuilder = new AutomaticPurchaseOrderResourceBuilder();

            this.Sut = new AutomaticPurchaseOrderFacadeService(
                this.AutomaticPurchaseOrderRepository,
                this.TransactionManager,
                this.AutomaticPurchaseOrderBuilder,
                this.AutomaticPurchaseOrderService);
        }
    }
}
