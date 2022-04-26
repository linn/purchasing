namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsInInspectionReportServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDefaultParameters : ContextBase
    {
        private WhatsInInspectionReport result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport();
        }

        [Test]
        public void ShouldCallRepoWithCorrectParam()
        {
            this.WhatsInInspectionRepository.Received().GetWhatsInInspection(false);
        }

        [Test]
        public void ShouldNotIncludePartsWithNoOrderNumber()
        {
            this.result.PartsInInspection.Count().Should().Be(3);
            this.result.PartsInInspection.All(x => this.OrdersData.Select(o => o.PartNumber).Contains(x.PartNumber))
                .Should().BeTrue();
        }

        [Test]
        public void ShouldCallOrdersDataRepoOnce()
        {
            this.WhatsInInspectionPurchaseOrdersDataRepository.Received(1).FilterBy(
                Arg.Any<Expression<Func<WhatsInInspectionPurchaseOrdersData, bool>>>());
        }

        [Test]
        public void ShouldOrderByMinDate()
        {
            for (var i = 1; i < this.result.PartsInInspection.Count(); i++)
            {
                var previousDate = this.result.PartsInInspection.ElementAt(i - 1).MinDate;
                this.result.PartsInInspection.ElementAt(i).MinDate.Should()
                    .BeOnOrAfter(previousDate.GetValueOrDefault());
            }
        }

        [Test]
        public void ShouldBuildOrdersDataReports()
        {
            this.result.PartsInInspection
                .All(x => x.OrdersBreakdown != null).Should().BeTrue();
        }

        [Test]
        public void ShouldBuildLocationsDataReports()
        {
            this.result.PartsInInspection
                .All(x => x.LocationsBreakdown != null).Should().BeTrue();
        }

        [Test]
        public void ShouldBuildBackOrdersReport()
        {
            this.result.BackOrderData.Should().NotBeNull();
        }
    }
}
