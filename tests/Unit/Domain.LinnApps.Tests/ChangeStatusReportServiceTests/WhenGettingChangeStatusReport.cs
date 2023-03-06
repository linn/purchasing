using Linn.Purchasing.Domain.LinnApps.Boms;

namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeStatusReportServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingChangeStatusReport : ContextBase
    {
        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetChangeStatusReport(6);
        }

        [Test]
        public void ShouldGetData()
        {
            this.ChangeRequestRepository.Received().FilterBy(Arg.Any<Expression<Func<ChangeRequest, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Rows.Count().Should().Be(3);
            this.results.GetGridTextValue(0, 1).Should().Be("ACCEPT ACCEPTED CHANGES");
            this.results.GetGridTextValue(1, 1).Should().Be("PROPOS PROPOSED CHANGES");
            this.results.GetGridTextValue(2, 1).Should().Be("TOTAL OUTSTANDING CHANGES");
        }
    }
}
