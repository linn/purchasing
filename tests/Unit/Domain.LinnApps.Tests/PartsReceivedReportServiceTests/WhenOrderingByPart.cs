namespace Linn.Purchasing.Domain.LinnApps.Tests.PartsReceivedReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenOrderingByPart : ContextBase
    {
        private ResultsModel results;

        private IEnumerable<PartsReceivedViewModel> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<PartsReceivedViewModel>
                            {
                                new PartsReceivedViewModel
                                    {
                                        SupplierName = "SUPPLIER 1",
                                        PartNumber = "PART C",
                                        MaterialPrice = 11,
                                        DateBooked = DateTime.UnixEpoch,
                                        OrderNumber = "100 / 1",
                                        JobRef = "AAA",
                                        OverStockValue = 1,
                                        OverstockQty = 1,
                                        PartPrice = 1,
                                        TqmsGroup = "LINN",
                                        Qty = 1
                                    },
                                new PartsReceivedViewModel
                                    {
                                        SupplierName = "SUPPLIER 2",
                                        PartNumber = "PART A",
                                        MaterialPrice = 22,
                                        DateBooked = DateTime.UnixEpoch.AddDays(1),
                                        OrderNumber = "200 / 1",
                                        JobRef = "AAA",
                                        OverStockValue = 2,
                                        OverstockQty = 2,
                                        PartPrice = 2,
                                        TqmsGroup = "LINN",
                                        Qty = 2
                                    },
                                new PartsReceivedViewModel
                                    {
                                        SupplierName = "SUPPLIER 3",
                                        PartNumber = "PART B",
                                        MaterialPrice = 33,
                                        DateBooked = DateTime.UnixEpoch.AddDays(2),
                                        OrderNumber = "300 / 1",
                                        JobRef = "AAA",
                                        OverStockValue = 3,
                                        OverstockQty = 3,
                                        PartPrice = 3,
                                        TqmsGroup = "LINN",
                                        Qty = -3
                                    }
                            };
            this.PartsReceivedView.FilterBy(Arg.Any<Expression<Func<PartsReceivedViewModel, bool>>>())
                .Returns(this.data.AsQueryable());

            this.results = this.Sut.GetReport(
                "AAA",
                null,
                DateTime.UnixEpoch.ToString("o"),
                DateTime.Today.ToString("o"),
                "PART");
        }

        [Test]
        public void ShouldOrderByPart()
        {
            for (var i = 1; i < this.results.Rows.Count(); i++)
            {
                var previousPart = this.results.GetGridTextValue(i - 1, 0);
                Assert.IsTrue(string.CompareOrdinal(this.results.GetGridTextValue(i, 0), previousPart) > 0);
            }
        }
    }
}
