namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    
    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingChangeRequest : ContextBase
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
                                  ChangeState = "PROPOS",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Test Change"
                              };
            this.Repository.FindById(1).Returns(request);
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
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldChangeStatusToAccepted()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.DocumentNumber.Should().Be(1);
            resource.ChangeState.Should().Be("ACCEPT");
        }
    }
}
