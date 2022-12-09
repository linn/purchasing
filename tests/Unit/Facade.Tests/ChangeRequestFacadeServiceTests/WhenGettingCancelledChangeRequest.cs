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

    public class WhenGettingCancelledChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.authorisationService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var request = new ChangeRequest()
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "CANCEL",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Cancelled already"
                              };
            this.repository.FindById(1).Returns(request);
            this.result = this.Sut.GetById(1, new List<string>() { "superpowers" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldBeCancelledWithAppropriateLinks()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.DocumentNumber.Should().Be(1);
            resource.ChangeState.Should().Be("CANCEL");
            resource.Links.Length.Should().Be(1);
            var acceptLink = resource.Links.SingleOrDefault(r => r.Rel == "approve");
            acceptLink.Should().BeNull();
            var cancelLink = resource.Links.SingleOrDefault(r => r.Rel == "cancel");
            cancelLink.Should().BeNull();
        }
    }
}
