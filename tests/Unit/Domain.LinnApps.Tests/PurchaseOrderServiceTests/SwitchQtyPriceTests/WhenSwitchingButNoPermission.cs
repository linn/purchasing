namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests.SwitchQtyPriceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSwitchingButNoPermission : SwitchQtyPriceContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseLedgerAdmin, Arg.Any<List<string>>())
                .Returns(false);

            this.Action = () => this.Sut.SwitchOurQtyAndPrice(this.OrderNumber, 1, this.EmployeeId, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.Action.Should().Throw<UnauthorisedActionException>().WithMessage("No permission to switch qty and price.");
        }
    }
}
