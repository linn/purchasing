namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;
    using System.Globalization;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenOrderingByDate : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.Today, "EXPECTED DATE", string.Empty, null);
        }

        [Test]
        public void ShouldOrderByExpectedDate()
        {
            for (var i = 1; i < this.result.Rows.Count(); i++)
            {
                var previousDateBooked = DateTime.ParseExact(
                    this.result.GetGridTextValue(i - 1, 5), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime.Parse(this.result.GetGridTextValue(i, 5)).Should().BeOnOrAfter(previousDateBooked);
            }
        }
    }
}
