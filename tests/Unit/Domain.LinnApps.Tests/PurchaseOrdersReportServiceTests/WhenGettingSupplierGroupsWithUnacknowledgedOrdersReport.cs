namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierGroupsWithUnacknowledgedOrdersReport : ContextBase
    {
        private ResultsModel result;

        private string vendorManger;

        private int? planner;

        private IQueryable<SupplierGroupsWithUnacknowledgedOrders> suppliers;

        [SetUp]
        public void SetUp()
        {
            this.planner = 1;
            this.vendorManger = "M";
            this.suppliers = new List<SupplierGroupsWithUnacknowledgedOrders>
                                 {
                                     new SupplierGroupsWithUnacknowledgedOrders
                                         {
                                             Id = 1, Name = "sg1", Planner = 1, VendorManager = "M", SupplierGroupId = 1, SupplierGroupName = "sg1"
                                         },
                                     new SupplierGroupsWithUnacknowledgedOrders
                                         {
                                             Id = 2, Name = "two", Planner = 1, VendorManager = "M"
                                         },
                                     new SupplierGroupsWithUnacknowledgedOrders
                                         {
                                             Id = 3, Name = "three", Planner = 1, VendorManager = "M"
                                         },
                                     new SupplierGroupsWithUnacknowledgedOrders
                                         {
                                             Id = 4, Name = "four",  Planner = 1, VendorManager = "B"
                                         },
                                     new SupplierGroupsWithUnacknowledgedOrders
                                         {
                                             Id = 5, Name = "five", Planner = 2, VendorManager = "M"
                                         }
                                 }.AsQueryable();
            this.SupplierGroupsWithUnacknowledgedOrdersRepository
                .FindAll().Returns(this.suppliers);
            this.result = this.Sut.GetSuppliersWithUnacknowledgedOrders(this.planner, this.vendorManger, true);
        }

        [Test]
        public void ShouldGetSuppliers()
        {
            this.SupplierGroupsWithUnacknowledgedOrdersRepository.Received().FindAll();
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Suppliers with unacknowledged orders");
            this.result.RowCount().Should().Be(3);
            this.result.ColumnCount().Should().Be(5);
            this.result.GetGridTextValue(0, 0).Should().Be("1");
            this.result.GetGridTextValue(0, 1).Should().Be("sg1");
            this.result.GetGridTextValue(0, 2).Should().Be("(Group)");
            this.result.GetGridTextValue(0, 3).Should().Be("view");
            this.result.GetGridTextValue(0, 4).Should().Be("csv");
            this.result.GetGridTextValue(1, 0).Should().Be("2");
            this.result.GetGridTextValue(1, 1).Should().Be("two");
            this.result.GetGridTextValue(1, 2).Should().Be(string.Empty);
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
