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

    public class WhenGettingReportByPurchaseLevel : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetShortagesReport(3, "ALL");
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
            // first VM
            firstPlannerResult.GetGridTextValue(0, 0).Should().Be("L");
            firstPlannerResult.GetGridTextValue(0, 1).Should().Be("Test VM L");
            firstPlannerResult.GetGridValue(0, 2).Should().Be(2);
            firstPlannerResult.GetGridValue(0, 3).Should().Be(1);
            // second VM
            firstPlannerResult.GetGridTextValue(1, 0).Should().Be("M");
            firstPlannerResult.GetGridTextValue(1, 1).Should().Be("Test VM M");
            firstPlannerResult.GetGridValue(1, 2).Should().Be(1);
            firstPlannerResult.GetGridValue(1, 3).Should().Be(2);
            // test the total
            firstPlannerResult.GetTotalValue(2).Should().Be(3);
            firstPlannerResult.GetTotalValue(3).Should().Be(3);

            var secondPlannerResult = this.results.ElementAt(1);
            secondPlannerResult.ReportTitle.DisplayValue.Should().Be("Second Test Planner");
            // first VM
            secondPlannerResult.GetGridTextValue(0, 0).Should().Be("L");
            secondPlannerResult.GetGridTextValue(0, 1).Should().Be("Test VM L");
            secondPlannerResult.GetGridValue(0, 2).Should().Be(2);
            // second VM
            secondPlannerResult.GetGridTextValue(1, 0).Should().Be("T");
            secondPlannerResult.GetGridTextValue(1, 1).Should().Be("Test VM T");
            secondPlannerResult.GetGridValue(1, 2).Should().Be(1);
            secondPlannerResult.GetGridValue(1, 3).Should().Be(2);
            // test the total
            secondPlannerResult.GetTotalValue(2).Should().Be(3);
            secondPlannerResult.GetTotalValue(3).Should().Be(2);
        }
    }
}
