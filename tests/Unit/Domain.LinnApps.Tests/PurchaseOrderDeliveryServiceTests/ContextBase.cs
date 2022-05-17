namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> Repository { get; private set; }

        protected IEnumerable<PurchaseOrderDelivery> Data { get; private set; }

        protected IPurchaseOrderDeliveryService Sut { get; private set; }

        protected IAuthorisationService AuthService { get; private set; }

        protected IRepository<RescheduleReason, string> RescheduleReasonRepository { get; private set; }

        protected ISingleRecordRepository<PurchaseLedgerMaster> PurchaseLedgerMaster { get; private set; }

        protected IRepository<MiniOrder, int> MiniOrderRepository { get; private set; }

        protected IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> MiniOrderDeliveryRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey>>();
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.RescheduleReasonRepository = Substitute.For<IRepository<RescheduleReason, string>>();
            this.PurchaseLedgerMaster = Substitute.For<ISingleRecordRepository<PurchaseLedgerMaster>>();
            this.MiniOrderRepository = Substitute.For<IRepository<MiniOrder, int>>();
            this.MiniOrderDeliveryRepository = Substitute.For<IRepository<MiniOrderDelivery, MiniOrderDeliveryKey>>();
            this.Data = PurchaseOrderDeliveryTestData.Data();
            this.Repository.FindAll().Returns(this.Data.AsQueryable());

            this.Sut = new PurchaseOrderDeliveryService(
                this.Repository, 
                this.AuthService, 
                this.RescheduleReasonRepository,
                this.PurchaseLedgerMaster,
                this.MiniOrderRepository,
                this.MiniOrderDeliveryRepository);
        }
    }
}
