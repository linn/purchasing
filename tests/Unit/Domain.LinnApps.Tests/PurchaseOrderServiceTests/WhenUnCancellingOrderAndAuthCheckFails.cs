namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NUnit.Framework;

    public class WhenUnCancellingOrderAndAuthCheckFails : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut.UnCancelOrder(1, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
