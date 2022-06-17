namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<MrPurchaseOrderDetail> data;

        private IEnumerable<ResultsModel> results;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<MrPurchaseOrderDetail>
                            {
                                new MrPurchaseOrderDetail
                                    {
                                        PartNumber = "PART A",
                                        OrderNumber = 1,
                                        Deliveries = new List<MrPurchaseOrderDelivery>
                                                         {
                                                            new MrPurchaseOrderDelivery
                                                                {
                                                                    OrderNumber = 1,
                                                                    DeliverySequence = 1,
                                                                    Quantity = 1,
                                                                    QuantityReceived = 0
                                                                },
                                                            new MrPurchaseOrderDelivery
                                                                {
                                                                    OrderNumber = 1,
                                                                    DeliverySequence = 2,
                                                                    Quantity = 1,
                                                                    QuantityReceived = 0
                                                                }
                                                         }
                                    },
                                new MrPurchaseOrderDetail
                                    {
                                        PartNumber = "PART A",
                                        OrderNumber = 2,
                                        Deliveries = new List<MrPurchaseOrderDelivery>
                                                         {
                                                             new MrPurchaseOrderDelivery
                                                                 {
                                                                     OrderNumber = 2,
                                                                     DeliverySequence = 1,
                                                                     Quantity = 1,
                                                                     QuantityReceived = 0
                                                                 },
                                                             new MrPurchaseOrderDelivery
                                                                 {
                                                                     OrderNumber = 2,
                                                                     DeliverySequence = 2,
                                                                     Quantity = 1,
                                                                     QuantityReceived = 0
                                                                 }
                                                         }
                                    },
                                new MrPurchaseOrderDetail
                                    {
                                        PartNumber = "PART B",
                                        OrderNumber = 3,
                                        Deliveries = new List<MrPurchaseOrderDelivery>
                                                         {
                                                             new MrPurchaseOrderDelivery
                                                                 {
                                                                     OrderNumber = 3,
                                                                     DeliverySequence = 1,
                                                                     Quantity = 1,
                                                                     QuantityReceived = 0
                                                                 }
                                                         }
                                    }
                            };
            this.Repository.FilterBy(Arg.Any<Expression<Func<MrPurchaseOrderDetail, bool>>>())
                .Returns(this.data.AsQueryable());
            this.MrMaster.GetRecord().Returns(new MrMaster());
            this.results = this.Sut.GetOrderBookReport(5000);
        }

        [Test]
        public void ShouldReturnReportForEachPartGroup()
        {
            this.results.Count().Should().Be(2);
            this.results.First().ReportTitle.DisplayValue.Should().Be("PART A");
            this.results.Last().ReportTitle.DisplayValue.Should().Be("PART B");
        }

        [Test]
        public void ShouldReturnLineForEachOrderDelivery()
        {
            this.results.First().Rows.Count().Should().Be(4);
            this.results.Last().Rows.Count().Should().Be(1);
        }
    }
}
