namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IReportReturnResourceBuilder Builder { get; private set; }

        protected IPurchaseOrdersReportService DomainService { get; private set; }

        protected IPurchaseOrderReportFacadeService Sut { get; private set; }

        protected IDeliveryPerformanceReportService DeliveryPerformanceReportService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DomainService = Substitute.For<IPurchaseOrdersReportService>();
            this.DeliveryPerformanceReportService = Substitute.For<IDeliveryPerformanceReportService>();
            this.Builder = new ReportReturnResourceBuilder();

            this.Sut = new PurchaseOrderReportFacadeService(
                this.DomainService,
                this.Builder,
                this.DeliveryPerformanceReportService);
        }
    }
}
