namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSuppliersWithUnacknowledgedOrdersReport : ContextBase
    {
        private ResultsModel result;

        private string vendorManger;

        private int? planner;

        private IQueryable<SuppliersWithUnacknowledgedOrders> suppliers;

        [SetUp]
        public void SetUp()
        {
            this.planner = 1;
            this.vendorManger = "M";
            this.suppliers = new List<SuppliersWithUnacknowledgedOrders>
                                 {
                                     new SuppliersWithUnacknowledgedOrders
                                         {
                                             SupplierId = 1, SupplierName = "one", Planner = 1, VendorManager = "M"
                                         },
                                     new SuppliersWithUnacknowledgedOrders
                                         {
                                             SupplierId = 2, SupplierName = "two", Planner = 1, VendorManager = "M"
                                         },
                                     new SuppliersWithUnacknowledgedOrders
                                         {
                                             SupplierId = 3, SupplierName = "three", Planner = 1, VendorManager = "M"
                                         },
                                     new SuppliersWithUnacknowledgedOrders
                                         {
                                             SupplierId = 4, SupplierName = "four",  Planner = 1, VendorManager = "B"
                                         },
                                     new SuppliersWithUnacknowledgedOrders
                                         {
                                             SupplierId = 5, SupplierName = "five", Planner = 2, VendorManager = "M"
                                         }
                                 }.AsQueryable();
            this.SuppliersWithUnacknowledgedOrdersRepository
                .FindAll().Returns(this.suppliers);
            this.result = this.Sut.GetSuppliersWithUnacknowledgedOrders(this.planner, this.vendorManger, false);
        }

        [Test]
        public void ShouldGetSuppliers()
        {
            this.SuppliersWithUnacknowledgedOrdersRepository.Received().FindAll();
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Suppliers with unacknowledged orders");
            this.result.RowCount().Should().Be(3);
            this.result.ColumnCount().Should().Be(5);
            this.result.GetGridTextValue(0, 0).Should().Be("1");
            this.result.GetGridTextValue(0, 1).Should().Be("one");
            this.result.GetGridTextValue(0, 2).Should().Be(string.Empty);
            this.result.GetGridTextValue(0, 3).Should().Be("view");
            this.result.GetGridTextValue(0, 4).Should().Be("csv");
            this.result.GetGridTextValue(1, 0).Should().Be("2");
            this.result.GetGridTextValue(1, 1).Should().Be("two");
            this.result.GetGridTextValue(2, 0).Should().Be("3");
            this.result.GetGridTextValue(2, 1).Should().Be("three");
        }

        [Test]
        public void ShouldSortReport()
        {
            this.result.Rows.First(a => a.RowId == "1").SortOrder.Should().Be(0);
            this.result.Rows.First(a => a.RowId == "2").SortOrder.Should().Be(2);
            this.result.Rows.First(a => a.RowId == "3").SortOrder.Should().Be(1);
        }
    }
}
