namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.UnixEpoch.AddDays(1), string.Empty, string.Empty, null);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be(
                $"Stock controlled parts due in between 1/1/1970 and 2/1/1970");
            this.result.Rows.Count().Should().Be(4);
        }

        [Test]
        public void ShouldSetCorrectExpectedDate()
        {
            foreach (var resultRow in this.result.Rows)
            {
                var dataRows = this.Repository.FilterBy(x => true);
                var dataRow = dataRows.First(
                    d => d.OrderNumber.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 0)
                         && d.OrderLine.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 1)
                         && d.DeliverySeq.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 2));
                var dateRequested = (DateTime)dataRow.DateRequested.GetValueOrDefault();
                var dateAdvised = (DateTime)dataRow.DateAdvised.GetValueOrDefault();

                // should use DateRequested if DateAdvised is null
                this.result.GetGridTextValue(resultRow.RowIndex, 5).Should().Be(
                    dataRow.DateAdvised == null
                        ? $"{dateRequested.Day}/{dateRequested.Month}/{dateRequested.Year}"
                        : $"{dateAdvised.Day}/{dateAdvised.Month}/{dateAdvised.Year}");
            }
        }

        [Test]
        public void ShouldCalculateTotals()
        {
            foreach (var resultRow in this.result.Rows)
            {
                var dataRows = this.Repository.FilterBy(x => true);
                var dataRow = dataRows.First(
                    d => d.OrderNumber.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 0)
                         && d.OrderLine.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 1)
                         && d.DeliverySeq.ToString() == this.result.GetGridTextValue(resultRow.RowIndex, 2));

                this.result.GetGridValue(resultRow.RowIndex, 7).Should()
                    .Be(dataRow.QuantityOutstanding * dataRow.BaseOurUnitPrice); // should be product of price and qty for every row
            }
        }

        [Test]
        public void ShouldOrderByOrderNumber()
        {
            for (var i = 1; i < this.result.Rows.Count(); i++)
            {
                var previousOrderNumber = this.result.GetGridTextValue(i - 1, 0);
                Assert.IsTrue(string.CompareOrdinal(this.result.GetGridTextValue(i, 0), previousOrderNumber) > 0);
            }
        }
    }
}
