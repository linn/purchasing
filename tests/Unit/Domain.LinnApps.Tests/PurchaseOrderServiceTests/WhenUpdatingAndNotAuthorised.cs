namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndNotAuthorised : ContextBase
    {
        private Action action;

        private PurchaseOrder current;

        private PurchaseOrder updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentType = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                SupplierId = 1224
            };
            this.updated = new PurchaseOrder 
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentType = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = "Y",
                OverbookQty = 1,
                SupplierId = 1224
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(false);

            this.action = () => this.Sut.AllowOverbook(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.current.Overbook.Should().Be(string.Empty);
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
