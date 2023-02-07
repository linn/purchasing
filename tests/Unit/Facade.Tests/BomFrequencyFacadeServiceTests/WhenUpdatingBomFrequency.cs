namespace Linn.Purchasing.Facade.Tests.BomFrequencyFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingBomFrequency : ContextBase
    {
        private IResult<BomFrequencyWeeksResource> result;

        [SetUp]
        public void SetUp()
        {
            var updateResource = new BomFrequencyWeeksResource
                                    {
                                        PartNumber = "CAP 161",
                                        FreqWeeks = 16
                                    };

            var request = new BomFrequencyWeeks
                             {
                                PartNumber = "CAP 161",
                                FreqWeeks = 2
                             };

            this.Repository.FindById(request.PartNumber).Returns(request);

            this.result = this.Sut.Update(request.PartNumber, updateResource, new List<string> { "self" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<BomFrequencyWeeksResource>>();
        }

        [Test]
        public void ShouldCommitChanges()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnChangeRequestWithChangedFields()
        {
            var resource = ((SuccessResult<BomFrequencyWeeksResource>)this.result).Data;
            resource.PartNumber.Should().Be("CAP 161");
            resource.FreqWeeks.Should().Be(16);
        }
    }
}
