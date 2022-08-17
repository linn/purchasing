namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<SupplierDeliveryPerformance> DeliveryPerformanceRepository { get; private set; }

        protected IQueryRepository<DeliveryPerformanceDetail> DeliveryPerformanceDetailRepository { get; private set; }
        
        protected IRepository<LedgerPeriod, int> LedgerPeriodRepository { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected DeliveryPerformanceReportService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DeliveryPerformanceRepository = Substitute.For<IQueryRepository<SupplierDeliveryPerformance>>();
            this.DeliveryPerformanceDetailRepository = Substitute.For<IQueryRepository<DeliveryPerformanceDetail>>();
            this.LedgerPeriodRepository = Substitute.For<IRepository<LedgerPeriod, int>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.ReportingHelper = new ReportingHelper();

            this.Sut = new DeliveryPerformanceReportService(
                this.DeliveryPerformanceRepository,
                this.DeliveryPerformanceDetailRepository,
                this.LedgerPeriodRepository,
                this.SupplierRepository,
                this.ReportingHelper);
        }
    }
}
