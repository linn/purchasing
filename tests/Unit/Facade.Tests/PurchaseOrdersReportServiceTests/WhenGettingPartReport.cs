namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
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

    public class WhenGettingPartReport : ContextBase
    {
        private readonly string partNumber = "RAW 123";

        private IResult<ReportReturnResource> result;

        [SetUp]
        public void SetUp()
        {
            var resource = new OrdersByPartSearchResource
                               {
                                   From = "01-Jan-2021", To = "01-Jun-2021", PartNumber = this.partNumber, Cancelled = "Y"
                               };
            this.DomainService.GetOrdersByPartReport(1.January(2021), 1.June(2021), this.partNumber, true).Returns(
                new ResultsModel
                    {
                        ReportTitle = new NameModel("Purchase Orders By Part: RAW 123")
                    });

            this.result = this.Sut.GetOrdersByPartReport(resource, new List<string>());
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.DomainService.Received().GetOrdersByPartReport(1.January(2021), 1.June(2021), this.partNumber, true);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<ReportReturnResource>>();
            var dataResult = ((SuccessResult<ReportReturnResource>) this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should()
                .Be("Purchase Orders By Part: RAW 123");
        }
    }
}
