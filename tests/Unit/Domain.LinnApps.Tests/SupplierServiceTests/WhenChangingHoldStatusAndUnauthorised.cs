namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingHoldStatusAndUnauthorised : UnauthorisedContext
    {
        private Action action;

        private SupplierOrderHoldHistoryEntry candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new SupplierOrderHoldHistoryEntry();

            this.MockAuthorisationService.HasPermissionFor(AuthorisedAction.SupplierCreate, Arg.Any<IEnumerable<string>>())
                .Returns(false);

            this.action = () => this.Sut.ChangeSupplierHoldStatus(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
