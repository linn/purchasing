namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndNotAuthorised : ContextBase
    {
        private readonly int reqNumber = 5678;

        private Action action;

        private PurchaseOrderReq current;

        private PurchaseOrderReq updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrderReq
                               {
                ReqNumber = this.reqNumber
            };
            this.updated = new PurchaseOrderReq
            {
                ReqNumber = this.reqNumber
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(false);

            this.action = () => this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
