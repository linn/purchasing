﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartsReceivedReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenOrderingBySupplier : ContextBase
    {
        private ResultsModel results;

        private IEnumerable<PartReceivedRecord> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<PartReceivedRecord>
                            {
                                new PartReceivedRecord
                                    {
                                        SupplierName = "SUPPLIER C",
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
                                new PartReceivedRecord
                                    {
                                        SupplierName = "SUPPLIER A",
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
                                new PartReceivedRecord
                                    {
                                        SupplierName = "SUPPLIER B",
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
            this.PartsReceivedView.FilterBy(Arg.Any<Expression<Func<PartReceivedRecord, bool>>>())
                .Returns(this.data.AsQueryable());

            this.results = this.Sut.GetReport(
                "AAA",
                null,
                DateTime.UnixEpoch.ToString("o"),
                DateTime.Today.ToString("o"),
                "SUPPLIER");
        }

        [Test]
        public void ShouldOrderBySupplier()
        {
            for (var i = 1; i < this.results.Rows.Count(); i++)
            {
                var previousSupplier = this.results.GetGridTextValue(i - 1, 3);
                Assert.That(string.CompareOrdinal(this.results.GetGridTextValue(i, 3), previousSupplier) > 0);
            }
        }
    }
}
