namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenFilteringByVendorManager : ContextBase
    {
        [Test]
        public void ShouldOnlyReturnMatching()
        {
            var result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.Today, string.Empty, "B", null);
            foreach (var row in result.Rows)
            {
                result.GetGridTextValue(row.RowIndex, 3).Should()
                    .Be("SUPPLIER with Vendor Manager B");
            }
        }
    }
}
