namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingAlreadyApprovedChangeRequest : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
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

            this.action = () => this.Sut.Approve(1, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<InvalidStateChangeException>();
        }
    }
}
