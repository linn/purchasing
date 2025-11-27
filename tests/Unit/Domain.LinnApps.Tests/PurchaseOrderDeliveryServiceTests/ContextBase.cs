namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderDeliveryRepository PurchaseOrderDeliveryRepository { get; private set; }

        protected IEnumerable<PurchaseOrderDelivery> Data { get; private set; }

        protected IPurchaseOrderDeliveryService Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected ISingleRecordRepository<PurchaseLedgerMaster> PurchaseLedgerMaster { get; private set; }

        protected IRepository<MiniOrder, int> MiniOrderRepository { get; private set; }

        protected IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> MiniOrderDeliveryRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IPurchaseOrdersPack PurchaseOrdersPack { get; private set; }

        protected IPurchaseOrderService PurchaseOrderService { get; private set; }

        protected ILog Log { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseOrderDeliveryRepository = Substitute.For<IPurchaseOrderDeliveryRepository>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PurchaseLedgerMaster = Substitute.For<ISingleRecordRepository<PurchaseLedgerMaster>>();
            this.MiniOrderRepository = Substitute.For<IRepository<MiniOrder, int>>();
            this.MiniOrderDeliveryRepository = Substitute.For<IRepository<MiniOrderDelivery, MiniOrderDeliveryKey>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.Data = PurchaseOrderDeliveryTestData.BuildData();
            this.PurchaseOrderDeliveryRepository.FindAll().Returns(this.Data.AsQueryable());
            this.PurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.PurchaseOrderService = Substitute.For<IPurchaseOrderService>();
            this.Log = Substitute.For<ILog>();

            this.Sut = new PurchaseOrderDeliveryService(
                this.PurchaseOrderDeliveryRepository, 
                this.AuthService, 
                this.PurchaseLedgerMaster,
                this.MiniOrderRepository,
                this.MiniOrderDeliveryRepository,
                this.PurchaseOrderRepository,
                this.PurchaseOrdersPack,
                this.PurchaseOrderService,
                this.Log);
        }
    }
}
