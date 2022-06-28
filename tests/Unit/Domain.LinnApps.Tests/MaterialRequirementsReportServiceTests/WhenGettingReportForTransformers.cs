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

    public class WhenGettingReportForTransformers : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.typeOfReport = "MR";
            this.partSelector = "Select Parts";
            this.partNumbers = new List<string> { "P1", "P2", "P3" };
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader>
                    {
                        new MrHeader { PartNumber = "P1" },
                        new MrHeader { PartNumber = "TRAN2" },
                        new MrHeader { PartNumber = "P3" },
                        new MrHeader { PartNumber = "TRAN 343" }
                    }.AsQueryable());
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                "All",
                "TRAN",
                "supplier/part",
                this.partNumbers);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(2);
            this.result.Headers.Should().Contain(a => a.PartNumber == "TRAN2");
            this.result.Headers.Should().Contain(a => a.PartNumber == "TRAN 343");
        }
    }
}
