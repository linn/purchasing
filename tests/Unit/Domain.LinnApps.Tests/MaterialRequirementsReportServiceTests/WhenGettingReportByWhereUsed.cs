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

    public class WhenGettingReportByWhereUsed : ContextBase
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
            this.partSelector = "Parts Where Used";
            this.partNumbers = new List<string> { "P1" };
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.PartsAndAssembliesRepository.FilterBy(Arg.Any<Expression<Func<PartAndAssembly, bool>>>()).Returns(
                x => new List<PartAndAssembly>
                         {
                             new PartAndAssembly { PartNumber = "P2", PartBomType = "C" },
                             new PartAndAssembly { PartNumber = "P3", PartBomType = "A" }
                         }.AsQueryable());
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader>
                    {
                        new MrHeader { PartNumber = "P1" },
                        new MrHeader { PartNumber = "P2" },
                        new MrHeader { PartNumber = "P3" }
                    }.AsQueryable());
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                null,
                null,
                "supplier/part",
                null,
                this.partNumbers);
        }

        [Test]
        public void ShouldDrillUpOneLevel()
        {
            this.PartsAndAssembliesRepository.Received(1).FilterBy(Arg.Any<Expression<Func<PartAndAssembly, bool>>>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(3);
            this.result.JobRef.Should().Be(this.jobRef);
            this.result.RunWeekNumber.Should().Be(this.runWeekNumber);
        }
    }
}
