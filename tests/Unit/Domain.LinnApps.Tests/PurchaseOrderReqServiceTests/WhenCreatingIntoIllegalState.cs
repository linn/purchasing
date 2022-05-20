namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderReqServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;

    using NUnit.Framework;

    public class WhenCreatingIntoIllegalState : ContextBase
    {
        private readonly int reqNumber = 5678;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut.Create(
                new PurchaseOrderReq { ReqNumber = this.reqNumber, State = "Order" },
                new List<string>());
        }

        [Test]
        public void ShouldThrowIllegalStateException()
        {
            this.action.Should().Throw<IllegalPoReqStateChangeException>();
        }
    }
}
