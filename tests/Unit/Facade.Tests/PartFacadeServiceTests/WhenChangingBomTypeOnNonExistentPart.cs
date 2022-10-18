namespace Linn.Purchasing.Facade.Tests.PartFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.InteropServices;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomTypeOnNonExistentPart : ContextBase
    {
        private IResult<BomTypeChangeResource> result;

        [SetUp]
        public void SetUp()
        {
            this.partService.ChangeBomType(Arg.Any<BomTypeChange>(), Arg.Any<IEnumerable<string>>()).Returns(x => throw new ItemNotFoundException("Part not found"));

            var request = new BomTypeChangeResource { PartNumber = "PIE IN SKY"};
            this.result = this.Sut.ChangeBomType(request);
        }

        [Test]
        public void ShouldReturnNotFound()
        {
            this.result.Should().BeOfType<NotFoundResult<BomTypeChangeResource>>();
            ((NotFoundResult<BomTypeChangeResource>) this.result).Message.Should().Be("Part not found");
        }
    }
}
