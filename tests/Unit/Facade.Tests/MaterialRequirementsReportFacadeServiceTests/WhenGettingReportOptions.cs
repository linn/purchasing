namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsReportFacadeServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportOptions : ContextBase
    {
        private IEnumerable<string> privileges;

        private IResult<MrReportOptionsResource> result;

        [SetUp]
        public void SetUp()
        {
            this.privileges = new List<string>();
            this.MaterialRequirementsReportService.GetOptions()
                .Returns(new MrReportOptions
                             {
                                 PartSelectorOptions = new List<ReportOption>
                                                         {
                                                             new ReportOption
                                                                 {
                                                                     DisplayText = "Option 1",
                                                                     Option = "1"
                                                                 }
                                                         },
                                 StockLevelOptions = new List<ReportOption>()
                             });

            this.result = this.Sut.GetOptions(this.privileges);
        }

        [Test]
        public void ShouldCallService()
        {
            this.MaterialRequirementsReportService.Received().GetOptions();
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<MrReportOptionsResource>>();
            var dataResult = ((SuccessResult<MrReportOptionsResource>)this.result).Data;
            dataResult.PartSelectorOptions.Should().HaveCount(1);
            dataResult.PartSelectorOptions.First().DisplayText.Should().Be("Option 1");
            dataResult.PartSelectorOptions.First().Option.Should().Be("1");
        }
    }
}
