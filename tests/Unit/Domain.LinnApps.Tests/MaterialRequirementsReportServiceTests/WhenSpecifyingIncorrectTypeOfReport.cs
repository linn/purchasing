namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSpecifyingIncorrectTypeOfReport : ContextBase
    {
        private string jobRef;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.typeOfReport = "does not exist";
            this.partSelector = "Select Parts";
            this.partNumbers = new List<string> { "P1", "P2" };
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader> { new MrHeader { PartNumber = "P1" }, new MrHeader { PartNumber = "P2" } }.AsQueryable());
            this.action = () => this.Sut.GetMaterialRequirements(
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
                null,
                0);
        }

        [Test]
        public void ShouldThrowError()
        {
            this.action.Should().Throw<InvalidOptionException>();
        }
    }
}
