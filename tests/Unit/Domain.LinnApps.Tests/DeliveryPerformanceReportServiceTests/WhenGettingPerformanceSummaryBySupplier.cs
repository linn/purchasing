﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
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

    public class WhenGettingPerformanceSummaryBySupplier : ContextBase
    {
        private readonly int startPeriod = 3;

        private readonly int endPeriod = 4;

        private readonly int? supplierId = 123;

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
            this.result = this.Sut.GetDeliveryPerformanceSummary(this.startPeriod, this.endPeriod, this.supplierId, this.vendorManager);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.DeliveryPerformanceRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierDeliveryPerformance, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Supplier Delivery Performance Summary");
            this.result.RowCount().Should().Be(2);
            this.result.GetGridTextValue(0, 0).Should().Be("Jan2028");
            this.result.GetGridTextValue(1, 0).Should().Be("Feb2028");

            this.result.GetGridValue(0, 1).Should().Be(10);
            this.result.GetGridValue(0, 2).Should().Be(10);
            this.result.GetGridValue(0, 3).Should().Be(100.0m);
            this.result.GetGridValue(0, 4).Should().Be(0);
            this.result.GetGridValue(0, 5).Should().Be(0);
            this.result.GetGridValue(0, 6).Should().Be(0);

            this.result.GetGridValue(1, 1).Should().Be(20);
            this.result.GetGridValue(1, 2).Should().Be(14);
            this.result.GetGridValue(1, 3).Should().Be(70.0m);
            this.result.GetGridValue(1, 4).Should().Be(2);
            this.result.GetGridValue(1, 5).Should().Be(2);
            this.result.GetGridValue(1, 6).Should().Be(2);

            this.result.GetTotalValue(1).Should().Be(30);
            this.result.GetTotalValue(2).Should().Be(24);
            this.result.GetTotalValue(3).Should().Be(80.0m);
            this.result.GetTotalValue(4).Should().Be(2);
            this.result.GetTotalValue(5).Should().Be(2);
            this.result.GetTotalValue(6).Should().Be(2);
        }
    }
}
