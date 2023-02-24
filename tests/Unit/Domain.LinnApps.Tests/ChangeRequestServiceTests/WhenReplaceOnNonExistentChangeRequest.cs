namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;

    using FluentAssertions;

    public class WhenReplaceOnNonExistentChangeRequest : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.Replace(1, 7004, true, null, null, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
