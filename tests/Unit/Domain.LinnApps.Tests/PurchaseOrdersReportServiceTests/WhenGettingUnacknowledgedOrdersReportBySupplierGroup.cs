namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersReportBySupplierGroup : ContextBase
    {
        private ResultsModel result;

        private int? supplierId;

        private int? supplierGroupId;

        private IQueryable<UnacknowledgedOrders> orders;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = null;
            this.supplierGroupId = 123;
            this.orders = new List<UnacknowledgedOrders>
                                 {
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = 2,
                                             OrganisationId = 456,
                                             OrderNumber = 1,
                                             OrderLine = 1,
                                             DeliveryNumber = 1,
                                             RequestedDate = 1.January(2029),
                                             OrderUnitPrice = 1.2m,
                                             PartNumber = "P1",
                                             SuppliersDesignation = "part 1",
                                             OrderDeliveryQuantity = 1m,
                                             SupplierGroupId = this.supplierGroupId,
                                             SupplierGroupName = "sg",
                                             CurrencyCode = "GBP"
                                         },
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = 3,
                                             OrganisationId = 456,
                                             OrderNumber = 2,
                                             OrderLine = 2,
                                             DeliveryNumber = 2,
                                             RequestedDate = 1.February(2029),
                                             OrderUnitPrice = 0.24356m,
                                             PartNumber = "P2",
                                             SuppliersDesignation = "part 2",
                                             OrderDeliveryQuantity = 300m,
                                             SupplierGroupId = this.supplierGroupId,
                                             SupplierGroupName = "sg",
                                             CurrencyCode = "EUR"
                                         }
                                 }.AsQueryable();
            this.UnacknowledgedOrdersRepository.FilterBy(Arg.Any<Expression<Func<UnacknowledgedOrders, bool>>>())
                .Returns(this.orders);
            this.SupplierGroupRepository.FindById(this.supplierGroupId.Value)
                .Returns(new SupplierGroup { Name = "Supplier Group Of Things" });
            this.result = this.Sut.GetUnacknowledgedOrders(this.supplierId, this.supplierGroupId);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.UnacknowledgedOrdersRepository.Received().FilterBy(Arg.Any<Expression<Func<UnacknowledgedOrders, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Unacknowledged orders for Supplier Group Of Things");
            this.result.RowCount().Should().Be(2);
            this.result.ColumnCount().Should().Be(8);
            this.result.Rows.First(a => a.RowId == "1/1/1").RowTitle.Should().Be("1");
            this.result.GetGridTextValue(0, 0).Should().Be("P1");
            this.result.GetGridTextValue(0, 1).Should().Be("part 1");
            this.result.GetGridValue(0, 2).Should().Be(1);
            this.result.GetGridValue(0, 3).Should().Be(1);
            this.result.GetGridValue(0, 4).Should().Be(1);
            this.result.GetGridTextValue(0, 5).Should().Be("GBP");
            this.result.GetGridTextValue(0, 6).Should().Be("1.20");
            this.result.GetGridTextValue(0, 7).Should().Be("01-Jan-2029");
            this.result.Rows.First(a => a.RowId == "2/2/2").RowTitle.Should().Be("2");
            this.result.GetGridTextValue(1, 0).Should().Be("P2");
            this.result.GetGridTextValue(1, 1).Should().Be("part 2");
            this.result.GetGridValue(1, 2).Should().Be(2);
            this.result.GetGridValue(1, 3).Should().Be(2);
            this.result.GetGridValue(1, 4).Should().Be(300);
            this.result.GetGridTextValue(1, 5).Should().Be("EUR");
            this.result.GetGridTextValue(1, 6).Should().Be("0.24356");
            this.result.GetGridTextValue(1, 7).Should().Be("01-Feb-2029");
        }
    }
}
