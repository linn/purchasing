namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportBySelectPartsNextChunk : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        private int reportChunk;

        [SetUp]
        public void SetUp()
        {
            this.reportChunk = 1;
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.typeOfReport = "MR";
            this.partSelector = "Select Parts";
            this.partNumbers = new List<string>();
            for (var i = 0; i < 101; i++)
            {
                this.partNumbers.Add(i.ToString("000"));
            }

            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            var headers = new List<MrHeader>();
            foreach (var partNumber in this.partNumbers)
            {
                headers.Add(new MrHeader { PartNumber = partNumber });
            }

            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>())
                .Returns(headers.AsQueryable());
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                null,
                null,
                "part",
                null,
                this.partNumbers,
                null,
                null,
                null,
                null,
                null,
                this.reportChunk);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(1);
            this.result.Headers.First().PartNumber.Should().Be("100");
            this.result.JobRef.Should().Be(this.jobRef);
            this.result.RunWeekNumber.Should().Be(this.runWeekNumber);
            this.result.ReportChunk.Should().Be(1);
            this.result.TotalChunks.Should().Be(2);
        }

        [Test]
        public void ShouldReturnSelectedOptions()
        {
            this.result.PartNumbersOption.Should().HaveCount(101);
        }
    }
}
