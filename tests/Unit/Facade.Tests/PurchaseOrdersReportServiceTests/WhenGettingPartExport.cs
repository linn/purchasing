namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartExport : ContextBase
    {
        private readonly string partNumber = "MCAS 5";

        private IEnumerable<IEnumerable<string>> csvData;

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

            this.csvData = this.Sut.GetOrdersByPartExport(resource, new List<string>());
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.DomainService.Received().GetOrdersByPartReport(1.January(2021), 1.June(2021), this.partNumber, true);
        }

        [Test]
        public void ShouldReturnCsv()
        {
            this.csvData.Should().BeOfType<IEnumerable<IEnumerable<string>>>();
        }
    }
}
