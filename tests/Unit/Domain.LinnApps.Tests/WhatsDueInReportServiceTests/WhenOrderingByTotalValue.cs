namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenOrderingByTotalValue : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.Today, "VALUE", string.Empty, null);
        }

        [Test]
        public void ShouldOrderByTotalValueDescending()
        {
            for (var i = 1; i < this.result.Rows.Count(); i++)
            {
                var currentValue = this.result.GetGridValue(i, 7);
                var previousValue = this.result.GetGridValue(i - 1, 7);
                currentValue.Should().BeLessOrEqualTo(previousValue ?? 7);
            }
        }
    }
}
