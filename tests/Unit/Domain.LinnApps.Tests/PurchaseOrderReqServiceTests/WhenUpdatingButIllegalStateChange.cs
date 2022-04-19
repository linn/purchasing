namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;
    using NSubstitute.ReturnsExtensions;

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

            this.MockReqsStateChangeRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderReqStateChange, bool>>>())
                .ReturnsNullForAnyArgs();
            this.action = () => this.Sut.Update(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<IllegalPoReqStateChangeException>();
        }
    }
}
