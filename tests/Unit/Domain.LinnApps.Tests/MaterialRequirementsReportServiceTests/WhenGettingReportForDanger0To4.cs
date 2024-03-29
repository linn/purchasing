﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportForDanger0To4 : ContextBase
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
            this.partNumbers = new List<string> { "P1", "P2", "P3", "P4", "P5", "P6" };
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader>
                    {
                        new MrHeader { PartNumber = "P1", DangerLevel = 0 },
                        new MrHeader { PartNumber = "P2", DangerLevel = 1 },
                        new MrHeader { PartNumber = "P3", DangerLevel = 2 },
                        new MrHeader { PartNumber = "P4", DangerLevel = 5 },
                        new MrHeader { PartNumber = "P5", DangerLevel = null },
                        new MrHeader { PartNumber = "P6", DangerLevel = 4 }
                    }.AsQueryable());
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                "0-4",
                null,
                "supplier/part",
                null,
                this.partNumbers,
                null,
                null,
                null,
                null,
                null,
                0);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(4);
            this.result.Headers.Should().Contain(a => a.PartNumber == "P1");
            this.result.Headers.Should().Contain(a => a.PartNumber == "P2");
            this.result.Headers.Should().Contain(a => a.PartNumber == "P3");
            this.result.Headers.Should().Contain(a => a.PartNumber == "P6");
            this.result.JobRef.Should().Be(this.jobRef);
            this.result.RunWeekNumber.Should().Be(this.runWeekNumber);
        }
    }
}
