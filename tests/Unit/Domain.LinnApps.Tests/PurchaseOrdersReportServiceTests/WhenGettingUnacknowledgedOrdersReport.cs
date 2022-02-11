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

    public class WhenGettingUnacknowledgedOrdersReport : ContextBase
    {
        private ResultsModel result;

        private int supplierId;

        private int organisationId;

        private IQueryable<UnacknowledgedOrders> orders;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 123;
            this.organisationId = 456;
            this.orders = new List<UnacknowledgedOrders>
                                 {
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = this.supplierId,
                                             OrganisationId = this.organisationId,
                                             OrderNumber = 1,
                                             OrderLine = 1,
                                             DeliveryNumber = 1,
                                             RequestedDate = 1.January(2029),
                                             OrderUnitPrice = 1.2m,
                                             PartNumber = "P1",
                                             SuppliersDesignation = "part 1",
                                             OrderDeliveryQuantity = 1m
                                         },
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = this.supplierId,
                                             OrganisationId = this.organisationId,
                                             OrderNumber = 2,
                                             OrderLine = 2,
                                             DeliveryNumber = 2,
                                             RequestedDate = 1.February(2029),
                                             OrderUnitPrice = 0.24356m,
                                             PartNumber = "P2",
                                             SuppliersDesignation = "part 2",
                                             OrderDeliveryQuantity = 300m
                                         }
                                 }.AsQueryable();
            this.UnacknowledgedOrdersRepository.FilterBy(Arg.Any<Expression<Func<UnacknowledgedOrders, bool>>>())
                .Returns(this.orders);
            this.SupplierRepository.FindById(this.supplierId).Returns(new Supplier { Name = "Supplier Of Things" });
            this.result = this.Sut.GetUnacknowledgedOrders(this.supplierId, this.organisationId);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.UnacknowledgedOrdersRepository.Received().FilterBy(Arg.Any<Expression<Func<UnacknowledgedOrders, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Unacknowledged orders for Supplier Of Things");
            this.result.RowCount().Should().Be(2);
            this.result.ColumnCount().Should().Be(7);
            this.result.GetGridTextValue(0, 0).Should().Be("1/1");
            this.result.GetGridTextValue(0, 1).Should().Be("P1");
            this.result.GetGridTextValue(0, 2).Should().Be("part 1");
            this.result.GetGridTextValue(0, 3).Should().Be("1");
            this.result.GetGridValue(0, 4).Should().Be(1);
            this.result.GetGridTextValue(0, 5).Should().Be("1.20");
            this.result.GetGridTextValue(0, 6).Should().Be("01-Jan-2029");
            this.result.GetGridTextValue(1, 0).Should().Be("2/2");
            this.result.GetGridTextValue(1, 1).Should().Be("P2");
            this.result.GetGridTextValue(1, 2).Should().Be("part 2");
            this.result.GetGridTextValue(1, 3).Should().Be("2");
            this.result.GetGridValue(1, 4).Should().Be(300);
            this.result.GetGridTextValue(1, 5).Should().Be("0.24356");
            this.result.GetGridTextValue(1, 6).Should().Be("01-Feb-2029");
        }
    }
}
