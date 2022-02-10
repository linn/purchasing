namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<Part> PartQueryRepository { get; private set; }

        protected IRepository<PurchaseLedger, int> PurchaseLedgerRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IPurchaseOrdersReportService Sut { get; private set; }

        protected IPurchaseOrdersPack PurchaseOrdersPack { get; private set; }

        protected IQueryRepository<SuppliersWithUnacknowledgedOrders> SuppliersWithUnacknowledgedOrdersRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseLedgerRepository = Substitute.For<IRepository<PurchaseLedger, int>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.PartQueryRepository = Substitute.For<IQueryRepository<Part>>();
            this.PurchaseOrdersPack = Substitute.For<IPurchaseOrdersPack>();
            this.SuppliersWithUnacknowledgedOrdersRepository = Substitute.For<IQueryRepository<SuppliersWithUnacknowledgedOrders>>();

            this.ReportingHelper = new ReportingHelper();
            this.Sut = new PurchaseOrdersReportService(
                this.PurchaseOrderRepository,
                this.SupplierRepository,
                this.PartQueryRepository,
                this.PurchaseLedgerRepository,
                this.PurchaseOrdersPack,
                this.ReportingHelper,
                this.SuppliersWithUnacknowledgedOrdersRepository);
        }
    }
}
