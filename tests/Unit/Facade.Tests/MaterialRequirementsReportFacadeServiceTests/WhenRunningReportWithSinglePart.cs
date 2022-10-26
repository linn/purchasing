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

    public class WhenRunningReportWithSinglePart : ContextBase
    {
        private IEnumerable<string> privileges;

        private MrRequestResource requestResource;

        private IResult<MrReportResource> result;

        private string jobRef;

        private string partNumber;

        [SetUp]
        public void SetUp()
        {
            this.jobRef = "ABC";
            this.partNumber = "P1";
            this.requestResource = new MrRequestResource
                                       {
                                           JobRef = this.jobRef,
                                           TypeOfReport = "MR",
                                           PartSelector = null,
                                           PartNumber = this.partNumber,
                                           PartNumbers = null,
                                           StockLevelSelector = "1",
                                           SupplierId = 123
                                       };
            this.privileges = new List<string>();

            this.MaterialRequirementsReportService.GetMaterialRequirements(
                    this.jobRef,
                    this.requestResource.TypeOfReport,
                    this.requestResource.PartSelector,
                    this.requestResource.StockLevelSelector,
                    this.requestResource.PartOption,
                    this.requestResource.OrderBySelector,
                    this.requestResource.SupplierId,
                    Arg.Is<IList<string>>(a => a.Contains("P1") && a.Count == 1),
                    this.requestResource.PartNumberList,
                    this.requestResource.StockCategoryName,
                    this.requestResource.MinimumLeadTimeWeeks,
                    this.requestResource.MinimumAnnualUsage,
                    null,
                    this.requestResource.ReportChunk)
                .Returns(
                    new MrReport
                        {
                            Headers = new List<MrHeader>
                                          {
                                              new MrHeader { JobRef = this.jobRef, MrDetails = new List<MrDetail>() }
                                          }
                        });

            this.result = this.Sut.GetMaterialRequirements(this.requestResource, this.privileges);
        }

        [Test]
        public void ShouldCallService()
        {
            this.MaterialRequirementsReportService.Received().GetMaterialRequirements(
                this.jobRef,
                this.requestResource.TypeOfReport,
                this.requestResource.PartSelector,
                this.requestResource.StockLevelSelector,
                this.requestResource.PartOption,
                this.requestResource.OrderBySelector,
                this.requestResource.SupplierId,
                Arg.Is<IList<string>>(a => a.Contains("P1") && a.Count == 1),
                this.requestResource.PartNumberList,
                this.requestResource.StockCategoryName,
                this.requestResource.MinimumLeadTimeWeeks,
                this.requestResource.MinimumAnnualUsage,
                null,
                this.requestResource.ReportChunk);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<MrReportResource>>();
            var dataResult = ((SuccessResult<MrReportResource>)this.result).Data;
            dataResult.Results.Should().HaveCount(1);
            dataResult.Results.First().JobRef.Should().Be(this.jobRef);
        }
    }
}
