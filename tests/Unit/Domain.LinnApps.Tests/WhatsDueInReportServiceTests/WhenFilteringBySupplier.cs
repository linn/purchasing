namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenFilteringBySupplier : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.Today, string.Empty, string.Empty, 2);
        }

        [Test]
        public void ShouldOnlyReturnMatching()
        {
            foreach (var row in result.Rows)
            {
                result.GetGridTextValue(row.RowIndex, 3).Should().Be("SUPPLIER 2");
            }
        }
    }
}
