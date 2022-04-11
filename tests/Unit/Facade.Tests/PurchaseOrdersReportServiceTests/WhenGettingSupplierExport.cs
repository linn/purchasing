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

    public class WhenGettingExport : ContextBase
    {
        private readonly int supplierId = 71234;

        private IEnumerable<IEnumerable<string>> csvData;

        [SetUp]
        public void SetUp()
        {
            var resource = new OrdersBySupplierSearchResource
                               {
                                   From = "01-Jan-2021",
                                   To = "01-Jun-2021",
                                   SupplierId = this.supplierId,
                                   Returns = "Y",
                                   Outstanding = "N",
                                   Cancelled = "Y",
                                   Credits = "Y",
                                   StockControlled = "A"
                               };
            this.DomainService
                .GetOrdersBySupplierReport(1.January(2021), 1.June(2021), this.supplierId, true, false, true, "Y", "A")
                .Returns(
                    new ResultsModel
                        {
                            ReportTitle = new NameModel("Purchase Orders By Supplier - 71234: Dwight K Schrute")
                        });

            this.csvData = this.Sut.GetOrdersBySupplierExport(resource, new List<string>());
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.DomainService.Received().GetOrdersBySupplierReport(
                1.January(2021),
                1.June(2021),
                this.supplierId,
                true,
                false,
                true,
                "Y",
                "A");
        }

        [Test]
        public void ShouldReturnCsv()
        {
            this.csvData.Should().BeOfType<IEnumerable<IEnumerable<string>>>();
        }
    }
}
