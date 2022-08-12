namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDeliveryPerformanceDetailsReportWithoutSupplierId : ContextBase
    {
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
                                           SupplierId = null
                                       };
            this.result = this.Sut.GetDeliveryPerformanceDetailReport(this.requestResource);
        }

        [Test]
        public void ShouldNotCallDomainService()
        {
            this.DeliveryPerformanceReportService
                .DidNotReceive()
                .GetDeliveryPerformanceDetails(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            var dataResult = (BadRequestResult<ReportReturnResource>)this.result;
            dataResult.Message.Should().Be("You must include a supplier id");
        }
    }
}
