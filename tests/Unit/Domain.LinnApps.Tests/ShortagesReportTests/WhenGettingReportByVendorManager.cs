namespace Linn.Purchasing.Domain.LinnApps.Tests.ShortagesReportTests
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

    public class WhenGettingReportByVendorManager : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetReport(3, "L");
        }


        [Test]
        public void ShouldGetData()
        {
            this.ShortagesRepository.Received().FilterBy(Arg.Any<Expression<Func<ShortagesEntry, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            var firstPlannerResult = this.results.First();
            firstPlannerResult.ReportTitle.DisplayValue.Should().Be("Test Planner");
            firstPlannerResult.GetGridTextValue(0, 0).Should().Be("L");
            firstPlannerResult.GetGridTextValue(0, 1).Should().Be("Test VM L");
            firstPlannerResult.GetGridValue(0, 2).Should().Be(2);
            firstPlannerResult.GetGridValue(0, 3).Should().Be(1);

            var secondPlannerResult = this.results.ElementAt(1);
            secondPlannerResult.ReportTitle.DisplayValue.Should().Be("Second Test Planner");
            secondPlannerResult.GetGridTextValue(0, 0).Should().Be("L");
            secondPlannerResult.GetGridTextValue(0, 1).Should().Be("Test VM L");
            secondPlannerResult.GetGridValue(0, 2).Should().Be(2);
        }
    }
}
