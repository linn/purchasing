﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;
    using NSubstitute.ReturnsExtensions;

    using NUnit.Framework;

    public class WhenUpdatingDeliveriesAndOrderLineNotFound : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PurchaseOrderRepository.FindById(1).ReturnsNull();
            this.action = () => this.Sut.UpdateDeliveriesForOrderLine(
                1,
                1,
                new List<PurchaseOrderDelivery>(),
                new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderDeliveryException>()
                .WithMessage("order line not found: 1 / 1.");
        }
    }
}
