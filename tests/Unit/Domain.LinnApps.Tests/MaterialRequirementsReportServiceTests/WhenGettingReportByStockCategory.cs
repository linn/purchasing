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

    public class WhenGettingReportByStockCategory : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private IList<string> partNumbers;

        private int runWeekNumber;

        private string stockCategoryName;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.typeOfReport = "MR";
            this.partSelector = "Stock Category";
            this.partNumbers = new List<string> { "P1", "P2" };
            this.stockCategoryName = "SC1";
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader>
                    {
                        new MrHeader { PartNumber = "P1", StockCategoryName = this.stockCategoryName },
                        new MrHeader { PartNumber = "P2", StockCategoryName = this.stockCategoryName }
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
                this.stockCategoryName);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(2);
            this.result.Headers.Should().Contain(a => a.PartNumber == "P1");
            this.result.Headers.Should().Contain(a => a.PartNumber == "P2");
        }
    }
}
