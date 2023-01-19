namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingApprovedChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.MakeLiveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Test Change"
                              };
            this.Repository.FindById(1).Returns(request);
            this.result = this.Sut.GetById(1, new List<string> { "superpowers" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldBeProposedWithAppropriateLinks()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.DocumentNumber.Should().Be(1);
            resource.ChangeState.Should().Be("ACCEPT");
            resource.Links.Length.Should().Be(4);
            resource.GlobalReplace.Should().BeFalse();
            var acceptLink = resource.Links.Single(r => r.Rel == "make-live");
            acceptLink.Should().NotBeNull();
            var cancelLink = resource.Links.Single(r => r.Rel == "cancel");
            cancelLink.Should().NotBeNull();
            var phaseInLink = resource.Links.Single(r => r.Rel == "phase-in");
            phaseInLink.Should().NotBeNull();
        }
    }
}
