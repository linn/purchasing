namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingNonExistentChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.authorisationService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.ApproveChangeRequest(1);
        }

        [Test]
        public void ShouldReturnNotFoundRequest()
        {
            this.result.Should().BeOfType<NotFoundResult<ChangeRequestResource>>();
            ((NotFoundResult<ChangeRequestResource>)this.result).Message.Should().Be("Change Request not found");
        }
    }
}
