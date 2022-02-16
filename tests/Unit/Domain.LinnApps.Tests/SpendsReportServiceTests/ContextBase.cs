namespace Linn.Purchasing.Domain.LinnApps.Tests.SpendsReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPurchaseLedgerPack PurchaseLedgerPack { get; private set; }

        protected IRepository<PurchaseLedger, int> PurchaseLedgerRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> PurchaseOrderRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IQueryRepository<SupplierSpend> SpendsRepository { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected ISpendsReportService Sut { get; private set; }

        protected IRepository<VendorManager, string> VendorManagerRepository { get; set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }


        [SetUp]
        public void SetUpContext()
        {
            this.PurchaseLedgerRepository = Substitute.For<IRepository<PurchaseLedger, int>>();
            this.PurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.SpendsRepository = Substitute.For<IQueryRepository<SupplierSpend>>();
            this.PurchaseLedgerPack = Substitute.For<IPurchaseLedgerPack>();
            this.VendorManagerRepository = Substitute.For<IRepository<VendorManager, string>>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();


            this.ReportingHelper = new ReportingHelper();

            this.Sut = new SpendsReportService(
                this.SpendsRepository,
                this.VendorManagerRepository,
                this.PurchaseLedgerPack,
                this.PurchaseOrderRepository,
                this.SupplierRepository,
                this.PartRepository,
                this.ReportingHelper);
        }
    }
}
