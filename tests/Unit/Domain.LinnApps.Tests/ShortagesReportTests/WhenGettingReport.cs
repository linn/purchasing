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

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetReport(3, "ALL");
        }


        [Test]
        public void ShouldGetData()
        {
            this.ShortagesRepository.Received().FilterBy(Arg.Any<Expression<Func<ShortagesEntry, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Should().HaveCount(2);
            this.results.FirstOrDefault().ReportTitle.DisplayValue.Should().Be("Purchasing shortages planner");
        }
    }
}
