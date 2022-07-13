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
    }
}
