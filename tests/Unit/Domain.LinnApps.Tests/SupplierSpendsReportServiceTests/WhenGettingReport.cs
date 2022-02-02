﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierSpendsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private readonly int supplierId = 77282;

        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var spends = new List<SupplierSpend>
                             {
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 120m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1290
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 130m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1288
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1273
                                     },
                             };


            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>()).Returns(spends.AsQueryable());

            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(1290);
            this.PurchaseLedgerPack.GetYearStartLedgerPeriod().Returns(1285);

            this.results = this.Sut.GetSpendBySupplierReport();
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseLedgerPack.Received().GetLedgerPeriod();
            this.PurchaseLedgerPack.Received().GetYearStartLedgerPeriod();
            this.SpendsRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should()
                .Be($"Spend by supplier report");
            this.results.Rows.Count().Should().Be(1);
            var row = this.results.Rows.First();
            row.RowId.Should().Be(this.supplierId.ToString());
            this.results.GetGridTextValue(0, 0).Should().Be(this.supplierId.ToString());
            this.results.GetGridTextValue(0, 1).Should().Be("seller1");
            this.results.GetGridTextValue(0, 2).Should().Be("120");
            this.results.GetGridTextValue(0, 3).Should().Be("250");
            this.results.GetGridTextValue(0, 4).Should().Be("500");
        }
    }
}
