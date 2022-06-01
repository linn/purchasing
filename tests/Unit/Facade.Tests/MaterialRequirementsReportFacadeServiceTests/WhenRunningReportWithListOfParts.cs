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

    public class WhenRunningReportWithListOfParts : ContextBase
    {
        private IEnumerable<string> privileges;

        private MrRequestResource requestResource;

        private IResult<MrReportResource> result;

        private string jobRef;

        [SetUp]
        public void SetUp()
        {
            this.jobRef = "ABC";
            this.requestResource = new MrRequestResource
                                       {
                                           JobRef = this.jobRef,
                                           TypeOfReport = "MR",
                                           PartSelector = "Select Parts",
                                           PartNumber = null,
                                           PartNumbers = new List<string> { "A", "B" },
                                           StockLevelSelector = "0-4"
                                       };
            this.privileges = new List<string>();

            this.MaterialRequirementsReportService.GetMaterialRequirements(
                    this.jobRef,
                    this.requestResource.TypeOfReport,
                    this.requestResource.PartSelector,
                    this.requestResource.StockLevelSelector,
                    Arg.Is<IList<string>>(a => a.Contains("A") && a.Contains("B")))
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
                Arg.Is<IList<string>>(a => a.Contains("A") && a.Contains("B")));
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
