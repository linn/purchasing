namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndNotAuthorised : ContextBase
    {
        private readonly int reqNumber = 5678;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PurchaseOrderReqCreate,
                Arg.Any<IEnumerable<string>>()).Returns(false);

            this.action = () => this.Sut.Create(
                new PurchaseOrderReq { ReqNumber = this.reqNumber },
                new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
