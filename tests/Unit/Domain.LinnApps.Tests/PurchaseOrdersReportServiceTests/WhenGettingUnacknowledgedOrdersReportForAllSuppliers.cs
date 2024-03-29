﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersReportForAllSuppliers : ContextBase
    {
        private ResultsModel result;

        private IQueryable<UnacknowledgedOrders> orders;

        [SetUp]
        public void SetUp()
        {
            this.orders = new List<UnacknowledgedOrders>
                                 {
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = 1,
                                             OrganisationId = 400,
                                             OrderNumber = 1,
                                             OrderLine = 1,
                                             DeliveryNumber = 1,
                                             RequestedDate = 1.January(2029),
                                             OrderUnitPrice = 1.2m,
                                             PartNumber = "P1",
                                             SuppliersDesignation = "part 1",
                                             OrderDeliveryQuantity = 1m,
                                             CurrencyCode = "GBP"
                                         },
                                     new UnacknowledgedOrders
                                         {
                                             SupplierId = 2,
                                             OrganisationId = 500,
                                             OrderNumber = 2,
                                             OrderLine = 2,
                                             DeliveryNumber = 2,
                                             RequestedDate = 1.February(2029),
                                             OrderUnitPrice = 0.24356m,
                                             PartNumber = "P2",
                                             SuppliersDesignation = "part 2",
                                             OrderDeliveryQuantity = 300m,
                                             CurrencyCode = "GBP"
                                         }
                                 }.AsQueryable();
            this.UnacknowledgedOrdersRepository.FindAll().Returns(this.orders);
            this.result = this.Sut.GetUnacknowledgedOrders(null, null);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.UnacknowledgedOrdersRepository.Received().FindAll();
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("All unacknowledged orders");
            this.result.RowCount().Should().Be(2);
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
            this.result.GetGridTextValue(1, 5).Should().Be("GBP");
            this.result.GetGridTextValue(1, 6).Should().Be("0.24356");
            this.result.GetGridTextValue(1, 7).Should().Be("01-Feb-2029");
        }
    }
}
