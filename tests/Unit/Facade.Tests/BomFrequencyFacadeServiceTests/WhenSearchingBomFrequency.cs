namespace Linn.Purchasing.Facade.Tests.BomFrequencyFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingBomFrequency : ContextBase
    {
        private IResult<BomFrequencyWeeksResource> result;

        [SetUp]
        public void SetUp()
        {
            var request = new BomFrequencyWeeks
            {
                PartNumber = "CAP 2020",
                FreqWeeks = 208,
            };
            this.Repository.FindById(request.PartNumber).Returns(request);
            this.result = this.Sut.GetById(request.PartNumber, new List<string> { "Self" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<BomFrequencyWeeksResource>>();
        }

        [Test]
        public void ShouldReturnOneMatchingChangeRequest()
        {
            var resource = ((SuccessResult<BomFrequencyWeeksResource>)this.result).Data;
            resource.PartNumber.Should().Be("CAP 2020");
            resource.FreqWeeks.Should().Be(208);
        }
    }
}
