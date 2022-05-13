namespace Linn.Purchasing.Domain.LinnApps.Tests.MrUsedOnReportServiceTests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;
    using NSubstitute.ReturnsExtensions;

    using NUnit.Framework;

    public class WhenGettingReportAndNoResults : ContextBase
    {
        private ResultsModel result;

        private string partNumber;

        [SetUp]
        public void SetUp()
        {
            this.partNumber = "RES 426";

            this.MockMrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = "AAAAA" });
            this.MockRepository.FilterBy(Arg.Any<Expression<Func<MrUsedOnRecord, bool>>>())
                .ReturnsNull();
            this.result = this.Sut.GetUsedOn(this.partNumber);
        }

        [Test]
        public void ShouldReturnEmptyReport()
        {
            this.result.Rows.Count().Should().Be(0);
            this.result.ReportTitle.DisplayValue.Should().Be(
                    $"Part: {this.partNumber} - No results found for part.");
        }
    }
}
