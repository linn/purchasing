namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.OrdersBySupplierReportServiceTests
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBuilder<ResultsModel> Builder { get; private set; }

        protected IPurchaseOrdersReportService DomainService { get; private set; }

        protected IPurchaseOrderReportFacadeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DomainService = Substitute.For<IPurchaseOrdersReportService>();
            this.Builder = new ResultsModelResourceBuilder();

            this.Sut = new PurchaseOrderReportFacadeService(this.DomainService, this.Builder);
        }
    }
}
