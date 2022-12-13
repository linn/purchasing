namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingAlreadyApprovedChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Test Change"
                              };
            this.Repository.FindById(1).Returns(request);
            this.result = this.Sut.ApproveChangeRequest(1);
        }

        [Test]
        public void ShouldReturnBadRequest()
        {
            this.result.Should().BeOfType<BadRequestResult<ChangeRequestResource>>();
            ((BadRequestResult<ChangeRequestResource>)this.result).Message.Should().Be("Cannot approve this change request");
        }
    }
}
