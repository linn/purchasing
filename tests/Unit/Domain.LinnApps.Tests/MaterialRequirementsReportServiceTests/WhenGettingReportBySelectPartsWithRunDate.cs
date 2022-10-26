namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportBySelectPartsWithRunDate : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        private DateTime runDate;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = null;
            this.runDate = 1.December(2024);
            this.typeOfReport = "MR";
            this.partSelector = "Select Parts";
            this.partNumbers = new List<string> { "P1", "P2" };
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader> { new MrHeader { PartNumber = "P1" }, new MrHeader { PartNumber = "P2" } }.AsQueryable());

            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { JobRef = "XYZ", RunWeekNumber = this.runWeekNumber });
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                null,
                null,
                "supplier/part",
                null,
                this.partNumbers,
                null,
                null,
                null,
                null,
                this.runDate,
                0);
        }

        [Test]
        public void ShouldGetRunLog()
        {
            this.RunLogRepository.Received().FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>());
        }

        [Test]
        public void ShouldNotGetMasterRecord()
        {
            this.MrMasterRecordRepository.DidNotReceive().GetRecord();
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(2);
            this.result.JobRef.Should().Be("XYZ");
            this.result.RunWeekNumber.Should().Be(this.runWeekNumber);
        }

        [Test]
        public void ShouldReturnSelectedOptions()
        {
            this.result.PartSelectorOption.Should().Be("Select Parts");
            this.result.PartNumbersOption.Should().HaveCount(2);
            this.result.OrderByOption.Should().Be("supplier/part");
        }
    }
}
