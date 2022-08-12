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

    public class WhenGettingDeliveryPerformanceDetailsReport : ContextBase
    {
        private readonly int supplierId = 123;

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
                                           SupplierId = this.supplierId
                                       };
            this.DeliveryPerformanceReportService.GetDeliveryPerformanceDetails(
                    this.startPeriod,
                    this.endPeriod,
                    this.supplierId)
                .Returns(new ResultsModel { ReportTitle = new NameModel("Title") });
            this.result = this.Sut.GetDeliveryPerformanceDetailReport(this.requestResource);
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DeliveryPerformanceReportService
                .Received()
                .GetDeliveryPerformanceDetails(this.startPeriod, this.endPeriod, this.supplierId);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            var dataResult = ((SuccessResult<ReportReturnResource>)this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
