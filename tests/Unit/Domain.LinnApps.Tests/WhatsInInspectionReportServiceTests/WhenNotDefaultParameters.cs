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
        public void ShouldCallOrdersDataRepoAgainToIncludeFailedParts()
        {
            this.WhatsInInspectionPurchaseOrdersDataRepository.Received(2).FilterBy(
                Arg.Any<Expression<Func<WhatsInInspectionPurchaseOrdersData, bool>>>());
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
