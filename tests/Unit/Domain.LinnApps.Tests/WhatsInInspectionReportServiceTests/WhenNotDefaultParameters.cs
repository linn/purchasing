namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsInInspectionReportServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNotDefaultParameters : ContextBase
    {
        private WhatsInInspectionReport result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetReport(true, false, true, false, false);
        }

        [Test]
        public void ShouldCallRepoWithCorrectParam()
        {
            this.WhatsInInspectionRepository.Received().GetWhatsInInspection(true);
        }

        [Test]
        public void ShouldIncludePartsWithNoOrderNumber()
        {
            // PART D has no OrdersData entry
            this.result.PartsInInspection.Any(x => x.PartNumber.Equals("PART D")); 
        }

        [Test]
        public void ShouldCallOrdersDataRepoTwice()
        {
            // calls repo again to add the included QC = "FAILS" parts
            this.WhatsInInspectionPurchaseOrdersDataRepository.Received(2).FilterBy(
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
        public void ShouldNotBuildLocationsDataReports()
        {
            this.result.PartsInInspection
                .All(x => x.LocationsBreakdown == null).Should().BeTrue();
        }

        [Test]
        public void ShouldNotBuildBackOrdersReport()
        {
            this.result.BackOrderData.Should().BeNull();
        }
    }
}
