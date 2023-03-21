namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartExport : ContextBase
    {
        private readonly string partNumber = "MCAS 5";

        private IResult<IEnumerable<IEnumerable<string>>> result;

        [SetUp]
        public void SetUp()
        {
            var resource = new OrdersByPartSearchResource
                               {
                                   From = "01-Jan-2021",
                                   To = "01-Jun-2021",
                                   PartNumber = this.partNumber,
                                   Cancelled = "Y"
                               };
            this.DomainService
                .GetOrdersByPartReport(1.January(2021), 1.June(2021), this.partNumber, true)
                .Returns(
                    new ResultsModel
                        {
                            ReportTitle = new NameModel($"Purchase Orders By Part:{this.partNumber}")
                        });

            this.result = this.Sut.GetOrdersByPartExport(resource);
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.DomainService.Received().GetOrdersByPartReport(1.January(2021), 1.June(2021), this.partNumber, true);
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.result.Should().BeOfType<SuccessResult<IEnumerable<IEnumerable<string>>>>();
        }
    }
}
