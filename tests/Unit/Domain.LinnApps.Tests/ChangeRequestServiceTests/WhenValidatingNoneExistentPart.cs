namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingNoneExistentPart : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns((Part)null);
            this.AuthService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.Approve(1, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
