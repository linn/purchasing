namespace Linn.Purchasing.Domain.LinnApps.Tests.ShortagesPlannerReportTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetShortagesPlannerReport(1);
        }

        [Test]
        public void ShouldGetData()
        {
            this.ShortagesPlannerRepository.Received().FilterBy(Arg.Any<Expression<Func<ShortagesPlannerEntry, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Should().HaveCount(13);
            var firstPlanner = this.results.First();
            firstPlanner.ReportTitle.DisplayValue.Should().Be("Purchasing danger parts for planner 1. Purchase Danger Level 1");
        }

        [Test]
        public void ShouldBeInCorrectOrder()
        {
            this.results.ElementAt(0).ReportTitle.DisplayValue.Should().Be("Purchasing danger parts for planner 1. Purchase Danger Level 1");
            this.results.ElementAt(1).ReportTitle.DisplayValue.Should().Be("Second Test Part");
            this.results.ElementAt(2).ReportTitle.DisplayValue.Should().Be("Expected Deliveries");
            this.results.ElementAt(3).ReportTitle.DisplayValue.Should().Be("Test Part");
            this.results.ElementAt(4).ReportTitle.DisplayValue.Should().Be("Expected Deliveries");
            this.results.ElementAt(5).ReportTitle.DisplayValue.Should().Be("Purchasing danger parts for planner 1. Purchase Danger Level 2");
            this.results.ElementAt(6).ReportTitle.DisplayValue.Should().Be("Test Part");
            this.results.ElementAt(7).ReportTitle.DisplayValue.Should().Be("Expected Deliveries");
            this.results.ElementAt(8).ReportTitle.DisplayValue.Should().Be("Purchasing danger parts for planner 1. Purchase Danger Level 3");
            this.results.ElementAt(9).ReportTitle.DisplayValue.Should().Be("Fourth Test Part");
            this.results.ElementAt(10).ReportTitle.DisplayValue.Should().Be("Expected Deliveries");
            this.results.ElementAt(11).ReportTitle.DisplayValue.Should().Be("Third Test Part");
            this.results.ElementAt(12).ReportTitle.DisplayValue.Should().Be("Expected Deliveries");
        }
    }
}
