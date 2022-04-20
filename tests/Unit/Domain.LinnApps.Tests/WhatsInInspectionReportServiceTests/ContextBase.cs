namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsInInspectionReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected WhatsInInspectionReportService Sut { get; private set; }

        protected IWhatsInInspectionRepository WhatsInInspectionRepository { get; private set; }

        protected IQueryRepository<WhatsInInspectionPurchaseOrdersData> 
            WhatsInInspectionPurchaseOrdersDataRepository
        {
            get; private set;
        }

        protected IQueryRepository<WhatsInInspectionStockLocationsData> 
            WhatsInInspectionStockLocationsDataRepository
        {
            get; private set;
        }

        protected IQueryRepository<WhatsInInspectionBackOrderData> 
            WhatsInInspectionBackOrderDataRepository
        {
            get; private set;
        }

        protected IReportingHelper ReportingHelper
        {
            get; private set;
        }

        [SetUp]
        public void SetUpContext()
        {
            this.ReportingHelper = new ReportingHelper();
            this.WhatsInInspectionRepository = Substitute.For<IWhatsInInspectionRepository>();
            this.WhatsInInspectionPurchaseOrdersDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionPurchaseOrdersData>>();
            this.WhatsInInspectionStockLocationsDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionStockLocationsData>>();
            this.WhatsInInspectionBackOrderDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionBackOrderData>>();

            this.Sut = new WhatsInInspectionReportService(
                this.WhatsInInspectionRepository,
                this.WhatsInInspectionPurchaseOrdersDataRepository,
                this.WhatsInInspectionStockLocationsDataRepository,
                this.WhatsInInspectionBackOrderDataRepository,
                this.ReportingHelper);
        }
    }
}
