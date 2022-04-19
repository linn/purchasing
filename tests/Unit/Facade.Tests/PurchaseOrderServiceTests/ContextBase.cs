namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBuilder<PurchaseOrder> Builder { get; private set; }

        protected IPurchaseOrderService DomainService { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IRepository<OverbookAllowedByLog, int> OverbookAllowedByLogRepository { get; private set; }

        protected PurchaseOrderFacadeService Sut { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.OverbookAllowedByLogRepository = Substitute.For<IRepository<OverbookAllowedByLog, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.DomainService = Substitute.For<IPurchaseOrderService>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.Builder = new PurchaseOrderResourceBuilder(this.AuthService);
            this.Sut = new PurchaseOrderFacadeService(this.PurchaseOrderRepository, this.TransactionManager, this.Builder, this.DomainService, this.OverbookAllowedByLogRepository);
        }
    }
}
