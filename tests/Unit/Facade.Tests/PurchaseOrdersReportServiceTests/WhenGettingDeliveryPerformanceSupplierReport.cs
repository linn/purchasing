namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDeliveryPerformanceSupplierReport : ContextBase
    {
        private readonly int? supplierId = 123;

        private readonly string vendorManager = "VM1";

        private readonly int startPeriod = 123;

        private readonly int endPeriod = 234;

        private IResult<ReportReturnResource> result;

        private DeliveryPerformanceRequestResource requestResource;

        [SetUp]
        public void SetUp()
        {
            this.requestResource = new DeliveryPerformanceRequestResource
                                       {
                                           StartPeriod = this.startPeriod,
                                           EndPeriod = this.endPeriod,
                                           SupplierId = this.supplierId,
                                           VendorManager = this.vendorManager
                                       };
            this.DeliveryPerformanceReportService.GetDeliveryPerformanceBySupplier(
                    this.startPeriod,
                    this.endPeriod,
                    this.supplierId,
                    this.vendorManager)
                .Returns(new ResultsModel { ReportTitle = new NameModel("Title") });
            this.result = this.Sut.GetDeliveryPerformanceSupplierReport(this.requestResource);
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DeliveryPerformanceReportService
                .Received()
                .GetDeliveryPerformanceBySupplier(this.startPeriod, this.endPeriod, this.supplierId, this.vendorManager);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            var dataResult = ((SuccessResult<ReportReturnResource>)this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
