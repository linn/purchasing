namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;

    using FluentAssertions;

    public class WhenCancellingNonExistentChangeRequest : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.Cancel(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
