namespace Linn.Purchasing.Domain.LinnApps.Tests.PrefSupReceiptsReportTests
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

    public class WhenGettingReportJustGBP : ContextBase
    {
        private ResultsModel results;

        private IEnumerable<ReceiptPrefSupDiff> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<ReceiptPrefSupDiff>
                            {
                                new ReceiptPrefSupDiff
                                    {
                                        PlReceiptId = 1,
                                        OrderNumber = 1000,
                                        OrderLine = 1,
                                        Qty = 1,
                                        PartNumber = "MECH 001",
                                        DateBooked = new DateTime(2022, 4, 22),
                                        OrderCurrency = "EUR",
                                        CurrencyUnitPrice = 10,
                                        ReceiptBaseUnitPrice = 5,
                                        PrefsupCurrency = "EUR",
                                        PrefsupCurrencyUnitPrice = 8,
                                        PrefsupBaseUnitPrice = 4,
                                        Difference = 1
                                    }
                            };
            this.Repository.FilterBy(Arg.Any<Expression<Func<ReceiptPrefSupDiff, bool>>>())
                .Returns(this.data.AsQueryable());

            this.results = this.Sut.GetReport(new DateTime(2022, 4, 22), new DateTime(2022, 4, 23), true);
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be("Receipts vs Pref Sup Price");
            this.results.Rows.Count().Should().Be(1);
        }

        [Test]
        public void ShouldReturnJustGBPCurrencyFields()
        {
            this.results.GetGridTextValue(0, 6).Should().Be("5"); // Receipt GBP value
            this.results.GetGridTextValue(0, 7).Should().Be("4"); // Prefsup GBP value
        }
    }
}
