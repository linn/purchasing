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

    public class WhenGettingAcceptedChangesReport : ContextBase
    {
        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            this.results = this.Sut.GetAcceptedChangesReport(6);
        }

        [Test]
        public void ShouldGetData()
        {
            this.ChangeRequestRepository.Received(1).FilterBy(Arg.Any<Expression<Func<ChangeRequest, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Rows.Count().Should().Be(5);
        }
    }
}
