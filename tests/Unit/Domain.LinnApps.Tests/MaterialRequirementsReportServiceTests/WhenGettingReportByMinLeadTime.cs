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

    public class WhenGettingReportByMinLeadTime : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        private int leadTimeWeeks;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.leadTimeWeeks = 3;
            this.typeOfReport = "MR";
            this.partSelector = "Select Parts";
            this.partNumbers = new List<string> { "P1", "P2" };
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader>
                    {
                        new MrHeader { PartNumber = "P1", LeadTimeWeeks = 1 },
                        new MrHeader { PartNumber = "P2", LeadTimeWeeks = 4 },
                        new MrHeader { PartNumber = "P3", LeadTimeWeeks = this.leadTimeWeeks },
                        new MrHeader { PartNumber = "P4", LeadTimeWeeks = null }
                    }.AsQueryable());
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
                this.leadTimeWeeks,
                null,
                null,
                0);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(2);
            this.result.Headers.Should().Contain(a => a.PartNumber == "P2");
            this.result.Headers.Should().Contain(a => a.PartNumber == "P3");
        }

        [Test]
        public void ShouldReturnSelectedOptions()
        {
            this.result.MinimumLeadTimeWeeks.Should().Be(this.leadTimeWeeks);
        }
    }
}
