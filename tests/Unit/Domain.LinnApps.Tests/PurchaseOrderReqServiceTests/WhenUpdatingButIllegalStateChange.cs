namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingButIllegalStateChange : ContextBase
    {
        private readonly string fromState = "DRAFT";

        private readonly int reqNumber = 5678;

        private readonly string toState = "ORDER";

        private Action action;

        private PurchaseOrderReq current;

        private PurchaseOrderReq updated;

        [SetUp]
        public void SetUp()
        {
            this.current =
                new PurchaseOrderReq { ReqNumber = this.reqNumber, RequestedById = 999, State = this.fromState };
            this.updated = new PurchaseOrderReq { ReqNumber = this.reqNumber, State = this.toState };
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.PurchaseOrderReqUpdate,
                Arg.Any<IEnumerable<string>>()).Returns(true);

            this.MockPurchaseOrderReqsPack.StateChangeAllowed(this.fromState, this.toState).Returns(false);
            this.action = () => this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<IllegalPoReqStateChangeException>();
        }
    }
}
