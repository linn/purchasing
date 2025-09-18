namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;
    
    using Linn.Purchasing.Resources.RequestResources;
    
    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingNonExistentChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.ChangeStatus(
                new ChangeRequestStatusChangeResource
                {
                    Id = 1,
                    Status = "ACCEPT"
                }, 
                1234, 
                new List<string> { "some privilege" });
        }

        [Test]
        public void ShouldReturnNotFoundRequest()
        {
            this.result.Should().BeOfType<NotFoundResult<ChangeRequestResource>>();
            ((NotFoundResult<ChangeRequestResource>)this.result).Message.Should().Be("Change Request not found");
        }
    }
}
