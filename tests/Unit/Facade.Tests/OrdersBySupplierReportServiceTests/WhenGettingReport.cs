namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.OrdersBySupplierReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private readonly int supplierId = 71234;

        private IResult<ReportReturnResource> result;

        [SetUp]
        public void SetUp()
        {
            var resource = new OrdersBySupplierSearchResource
                               {
                                   From = "01-Jan-2021", To = "01-Jun-2021", SupplierId = this.supplierId, Returns = "Y", Outstanding = "N", Cancelled = "Y", Credits = "Y", StockControlled = "A"
                               };
            this.DomainService.GetOrdersBySupplierReport(1.January(2021), 1.June(2021), this.supplierId, true, false, true, "Y", "A").Returns(
                new ResultsModel
                    {
                        ReportTitle = new NameModel("Purchase Orders By Supplier - 71234: Dwight K Schrute")
                    });

            this.result = this.Sut.GetOrdersBySupplierReport(resource, new List<string>());
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.DomainService.Received().GetOrdersBySupplierReport(1.January(2021), 1.June(2021), this.supplierId, true, false, true, "Y", "A");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<ReportReturnResource>>();
            var dataResult = ((SuccessResult<ReportReturnResource>) this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should()
                .Be("Purchase Orders By Supplier - 71234: Dwight K Schrute");
        }
    }
}
