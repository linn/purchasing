namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseOrderDeliveryRepository Repository { get; private set; }

        protected IEnumerable<PurchaseOrderDelivery> Data { get; private set; }

        protected IPurchaseOrderDeliveryService Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected ISingleRecordRepository<PurchaseLedgerMaster> PurchaseLedgerMaster { get; private set; }

        protected IRepository<MiniOrder, int> MiniOrderRepository { get; private set; }

        protected IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> MiniOrderDeliveryRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IRepository<PlReceipt, int> ReceiptRepository { get; private set; }


        protected IPurchaseOrdersPack PurchaseOrdersPack { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IPurchaseOrderDeliveryRepository>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.PurchaseLedgerMaster = Substitute.For<ISingleRecordRepository<PurchaseLedgerMaster>>();
            this.MiniOrderRepository = Substitute.For<IRepository<MiniOrder, int>>();
            this.MiniOrderDeliveryRepository = Substitute.For<IRepository<MiniOrderDelivery, MiniOrderDeliveryKey>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.Data = PurchaseOrderDeliveryTestData.BuildData();
            this.Repository.FindAll().Returns(this.Data.AsQueryable());
            this.PurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.ReceiptRepository = Substitute.For<IRepository<PlReceipt, int>>();

            this.Sut = new PurchaseOrderDeliveryService(
                this.Repository, 
                this.AuthService, 
                this.PurchaseLedgerMaster,
                this.MiniOrderRepository,
                this.MiniOrderDeliveryRepository,
                this.PurchaseOrderRepository,
                this.PurchaseOrdersPack,
                this.ReceiptRepository);
        }
    }
}
