namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPerformanceBySupplier : ContextBase
    {
        private readonly int startPeriod = 3;

        private readonly int endPeriod = 4;

        private readonly int? supplierId = null;

        private readonly string vendorManager = null;

        private ResultsModel result;

        private List<SupplierDeliveryPerformance> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<SupplierDeliveryPerformance>
                            {
                                new SupplierDeliveryPerformance
                                    {
                                        LedgerPeriod = 3, 
                                        MonthName = "Jan2028", 
                                        SupplierId = 123,
                                        SupplierName = "S123",
                                        VendorManager = "A",
                                        NumberOfDeliveries = 10,
                                        NumberOnTime = 10,
                                        NumberOfEarlyDeliveries = 0,
                                        NumberOfUnacknowledgedDeliveries = 0,
                                        NumberOfLateDeliveries = 0
                                    },
                                new SupplierDeliveryPerformance
                                    {
                                        LedgerPeriod = 3,
                                        MonthName = "Jan2028",
                                        SupplierId = 808,
                                        SupplierName = "S808",
                                        VendorManager = "B",
                                        NumberOfDeliveries = 5,
                                        NumberOnTime = 3,
                                        NumberOfEarlyDeliveries = 0,
                                        NumberOfUnacknowledgedDeliveries = 0,
                                        NumberOfLateDeliveries = 2
                                    },
                                new SupplierDeliveryPerformance
                                    {
                                        LedgerPeriod = 4,
                                        MonthName = "Feb2028",
                                        SupplierId = 123,
                                        SupplierName = "S123",
                                        VendorManager = "A",
                                        NumberOfDeliveries = 20,
                                        NumberOnTime = 14,
                                        NumberOfEarlyDeliveries = 2,
                                        NumberOfUnacknowledgedDeliveries = 2,
                                        NumberOfLateDeliveries = 2
                                    },
                                new SupplierDeliveryPerformance
                                    {
                                        LedgerPeriod = 4,
                                        MonthName = "Feb2028",
                                        SupplierId = 808,
                                        SupplierName = "S808",
                                        VendorManager = "B",
                                        NumberOfDeliveries = 10,
                                        NumberOnTime = 10,
                                        NumberOfEarlyDeliveries = 0,
                                        NumberOfUnacknowledgedDeliveries = 0,
                                        NumberOfLateDeliveries = 0
                                    }
                            };
            this.DeliveryPerformanceRepository.FilterBy(Arg.Any<Expression<Func<SupplierDeliveryPerformance, bool>>>())
                .Returns(this.data.AsQueryable());
            this.result = this.Sut.GetDeliveryPerformanceBySupplier(this.startPeriod, this.endPeriod, this.supplierId, this.vendorManager);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.DeliveryPerformanceRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierDeliveryPerformance, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Delivery Performance By Supplier");
            this.result.RowCount().Should().Be(2);
            this.result.GetGridTextValue(0, 0).Should().Be("123");
            this.result.GetGridTextValue(0, 1).Should().Be("S123");
            this.result.GetGridTextValue(1, 0).Should().Be("808");
            this.result.GetGridTextValue(1, 1).Should().Be("S808");

            this.result.GetGridValue(0, 2).Should().Be(30);
            this.result.GetGridValue(0, 3).Should().Be(24);
            this.result.GetGridValue(0, 4).Should().Be(80.0m);
            this.result.GetGridValue(0, 5).Should().Be(2);
            this.result.GetGridValue(0, 6).Should().Be(2);
            this.result.GetGridValue(0, 7).Should().Be(2);

            this.result.GetGridValue(1, 2).Should().Be(15);
            this.result.GetGridValue(1, 3).Should().Be(13);
            this.result.GetGridValue(1, 4).Should().Be(86.7m);
            this.result.GetGridValue(1, 5).Should().Be(0);
            this.result.GetGridValue(1, 6).Should().Be(2);
            this.result.GetGridValue(1, 7).Should().Be(0);

            this.result.GetTotalValue(2).Should().Be(45);
            this.result.GetTotalValue(3).Should().Be(37);
            this.result.GetTotalValue(4).Should().Be(82.2m);
            this.result.GetTotalValue(5).Should().Be(2);
            this.result.GetTotalValue(6).Should().Be(4);
            this.result.GetTotalValue(7).Should().Be(2);
        }
    }
}
