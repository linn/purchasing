namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

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
                                        PartSupplierRecord = new PartSupplier
                                                                 {
                                                                     LeadTimeWeeks = 1,
                                                                     PartNumber = "PART A",
                                                                     Part = new Part
                                                                                {
                                                                                    Description = "Desc A",
                                                                                    PartNumber = "PART A",
                                                                                    OurUnitOfMeasure = "ONES"
                                                                                }
                                                                 },
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
                                        PartSupplierRecord = new PartSupplier
                                                                 {
                                                                     PartNumber = "PART A",
                                                                     LeadTimeWeeks = 1,
                                                                     Part = new Part
                                                                                {
                                                                                    Description = "Desc A",
                                                                                    PartNumber = "PART A",
                                                                                    OurUnitOfMeasure = "ONES"
                                                                                }
                                                                 },
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
                                        PartSupplierRecord = new PartSupplier
                                                                 {
                                                                     LeadTimeWeeks = 2,
                                                                     PartNumber = "PART B",
                                                                     Part = new Part
                                                                                {
                                                                                    Description = "Desc B",
                                                                                    OurUnitOfMeasure = "TWOS",
                                                                                    PartNumber = "PART B",
                                                                                }
                                                                 },
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
            this.results.First().ReportTitle.DisplayValue.Should().Be("PART A - Desc A - UOM: ONES - LEAD TIME: 1 WEEKS");
            this.results.Last().ReportTitle.DisplayValue.Should().Be("PART B - Desc B - UOM: TWOS - LEAD TIME: 2 WEEKS");
        }

        [Test]
        public void ShouldReturnLineForEachOrderDelivery()
        {
            this.results.First().Rows.Count().Should().Be(4);
            this.results.Last().Rows.Count().Should().Be(1);
        }

        [Test]
        public void ShouldOrderByPart()
        {
            for (int i = 1; i < this.results.Count(); i++)
            {
                var currentPart = this.results.ElementAt(i).ReportTitle.DisplayValue.Split(" _ ").First();
                var previousPart = this.results.ElementAt(i - 1).ReportTitle.DisplayValue.Split(" _ ").First();
                Assert.IsTrue(string.CompareOrdinal(currentPart, previousPart) > 0);
            }
        }
    }
}
