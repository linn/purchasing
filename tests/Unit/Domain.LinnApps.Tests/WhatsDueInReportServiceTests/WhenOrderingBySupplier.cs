namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsDueInReportServiceTests
{
    using System;
    using System.Linq;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenOrderingBySupplier : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(DateTime.UnixEpoch, DateTime.Today, "SUPPLIER", string.Empty, null);
        }

        [Test]
        public void ShouldOrderBySupplier()
        {
            for (var i = 1; i < this.result.Rows.Count(); i++)
            {
                var previousSupplier = this.result.GetGridTextValue(i - 1, 3);
                Assert.IsTrue(string.CompareOrdinal(this.result.GetGridTextValue(i, 3), previousSupplier) > 0);
            }
        }
    }
}
