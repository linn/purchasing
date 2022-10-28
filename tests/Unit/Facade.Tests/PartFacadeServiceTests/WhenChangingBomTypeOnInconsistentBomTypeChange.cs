namespace Linn.Purchasing.Facade.Tests.PartFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomTypeAndInconsistentBomTypeChange : ContextBase
    {
        private IResult<BomTypeChangeResource> result;

        [SetUp]
        public void SetUp()
        {
            this.partService.ChangeBomType(Arg.Any<BomTypeChange>(), Arg.Any<IEnumerable<string>>()).Returns(x => throw new InvalidBomTypeChangeException("Inconsistent Bom Type Change"));
            var request = new BomTypeChangeResource { PartNumber = "CONN 1", OldBomType = "C", NewBomType = "A" };
            this.result = this.Sut.ChangeBomType(request, new List<string>());
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.result.Should().BeOfType<BadRequestResult<BomTypeChangeResource>>();
            ((BadRequestResult<BomTypeChangeResource>)this.result).Message.Should().Be("Inconsistent Bom Type Change");
        }
    }
}
