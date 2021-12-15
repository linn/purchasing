namespace Linn.Purchasing.Domain.LinnApps.Tests.OrdersBySupplerReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IReportingHelper ReportingHelper { get; private set; }

        protected IRepository<PurchaseLedger, int> PurchaseLedgerRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IPurchaseOrdersReportService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseLedgerRepository = Substitute.For<IRepository<PurchaseLedger, int>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.ReportingHelper = Substitute.For<IReportingHelper>();
            this.Sut = new PurchaseOrdersReportService(
                this.PurchaseOrderRepository,
                this.SupplierRepository,
                this.PurchaseLedgerRepository,
                this.ReportingHelper);
        }
    }
}
