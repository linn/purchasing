namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;

    using FluentAssertions;

    public class WhenPhasingInNonExistentChangeRequest : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var bomChangeIds = new List<int> { 1 };

            this.action = () => this.Sut.PhaseInChanges(1, 1, new List<int>(), new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
