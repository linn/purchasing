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

    public class WhenGettingReportByPartNumberList : ContextBase
    {
        private string jobRef;

        private MrReport result;

        private string typeOfReport;

        private string partSelector;

        private int runWeekNumber;

        private string partNumberList;

        [SetUp]
        public void SetUp()
        {
            this.runWeekNumber = 1233;
            this.jobRef = "ABC";
            this.typeOfReport = "MR";
            this.partSelector = "Part Number List";
            this.partNumberList = "LIST";
            this.MrMasterRecordRepository.GetRecord().Returns(new MrMaster { JobRef = this.jobRef });
            this.RunLogRepository.FindBy(Arg.Any<Expression<Func<MrpRunLog, bool>>>())
                .Returns(new MrpRunLog { RunWeekNumber = this.runWeekNumber });
            this.MrHeaderRepository.FilterBy(Arg.Any<Expression<Func<MrHeader, bool>>>()).Returns(
                new List<MrHeader> { new MrHeader { PartNumber = "P1" }, new MrHeader { PartNumber = "P2" } }.AsQueryable());
            this.PartNumberListRepository.FindById(this.partNumberList).Returns(
                new PartNumberList
                    {
                        Name = this.partNumberList,
                        Elements = new List<PartNumberListElement>
                                       {
                                           new PartNumberListElement
                                               {
                                                   ListName = this.partNumberList, PartNumber = "P1"
                                               },
                                           new PartNumberListElement
                                               {
                                                   ListName = this.partNumberList, PartNumber = "P2"
                                               }
                                       }
                    });
            this.result = this.Sut.GetMaterialRequirements(
                this.jobRef,
                this.typeOfReport,
                this.partSelector,
                null,
                null,
                "supplier/part",
                null,
                null,
                this.partNumberList,
                null,
                null,
                null,
                0);
        }

        [Test]
        public void ShouldGetPartNumberList()
        {
            this.PartNumberListRepository.Received().FindById(this.partNumberList);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Headers.Should().HaveCount(2);
            this.result.JobRef.Should().Be(this.jobRef);
            this.result.RunWeekNumber.Should().Be(this.runWeekNumber);
        }

        [Test]
        public void ShouldReturnSelectedOptions()
        {
            this.result.PartSelectorOption.Should().Be(this.partSelector);
            this.result.PartNumberListOption.Should().Be(this.partNumberList);
        }
    }
}
