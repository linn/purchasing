namespace Linn.Purchasing.Domain.LinnApps.Tests.DeliveryPerformanceReportServiceTests
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

    public class WhenGettingPerformanceDetail : ContextBase
    {
        private readonly int startPeriod = 3;

        private readonly int endPeriod = 3;

        private readonly int supplierId = 12345;

        private ResultsModel result;

        private List<DeliveryPerformanceDetail> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<DeliveryPerformanceDetail>
                            {
                                new DeliveryPerformanceDetail
                                    {
                                        SupplierId = this.supplierId,
                                        DateArrived = 1.April(2024),
                                        OrderNumber = 100000,
                                        OrderLine = 10,
                                        DeliverySequence = 5,
                                        PartNumber = "P1",
                                        RequestedDate = 1.April(2024),
                                        AdvisedDate = 1.April(2024),
                                        RescheduleReason = "OK",
                                        OnTime = 1
                                    },
                                new DeliveryPerformanceDetail
                                    {
                                        SupplierId = this.supplierId,
                                        DateArrived = 30.April(2024),
                                        OrderNumber = 100010,
                                        OrderLine = 1,
                                        DeliverySequence = 3,
                                        PartNumber = "P2",
                                        RequestedDate = 1.April(2024),
                                        AdvisedDate = 1.April(2024),
                                        RescheduleReason = "Not OK",
                                        OnTime = 0
                                    }
                            };
            this.LedgerPeriodRepository.FindById(3).Returns(new LedgerPeriod { MonthName = "Apr2024" });
            this.SupplierRepository.FindById(this.supplierId)
                .Returns(new Supplier { Name = "Supplier 1" });
            this.DeliveryPerformanceDetailRepository
                .FilterBy(Arg.Any<Expression<Func<DeliveryPerformanceDetail, bool>>>())
                .Returns(this.data.AsQueryable());
            this.result = this.Sut.GetDeliveryPerformanceDetails(this.startPeriod, this.endPeriod, this.supplierId);
        }

        [Test]
        public void ShouldGetOrders()
        {
            this.DeliveryPerformanceDetailRepository.Received().FilterBy(Arg.Any<Expression<Func<DeliveryPerformanceDetail, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Delivery Performance Details for Supplier 1 Apr2024 to Apr2024");
            this.result.RowCount().Should().Be(2);
            this.result.GetGridTextValue(0, 0).Should().Be("100000");
            this.result.GetGridTextValue(0, 1).Should().Be("10");
            this.result.GetGridTextValue(0, 2).Should().Be("5");
            this.result.GetGridTextValue(1, 0).Should().Be("100010");
            this.result.GetGridTextValue(1, 1).Should().Be("1");
            this.result.GetGridTextValue(1, 2).Should().Be("3");

            this.result.GetGridTextValue(0, 3).Should().Be("P1");
            this.result.GetGridTextValue(0, 4).Should().Be("01-Apr-2024");
            this.result.GetGridTextValue(0, 5).Should().Be("01-Apr-2024");
            this.result.GetGridTextValue(0, 6).Should().Be("01-Apr-2024");
            this.result.GetGridTextValue(0, 7).Should().Be("OK");
            this.result.GetGridTextValue(0, 8).Should().Be("Yes");

            this.result.GetGridTextValue(1, 3).Should().Be("P2");
            this.result.GetGridTextValue(1, 4).Should().Be("01-Apr-2024");
            this.result.GetGridTextValue(1, 5).Should().Be("01-Apr-2024");
            this.result.GetGridTextValue(1, 6).Should().Be("30-Apr-2024");
            this.result.GetGridTextValue(1, 7).Should().Be("Not OK");
            this.result.GetGridTextValue(1, 8).Should().Be("No");
        }
    }
}
