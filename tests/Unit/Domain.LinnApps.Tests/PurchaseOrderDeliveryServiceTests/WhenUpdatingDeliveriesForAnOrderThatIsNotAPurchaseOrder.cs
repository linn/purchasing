﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingDeliveriesForAnOrderThatIsNotAPurchaseOrder : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });
            this.PurchaseOrderRepository.FindById(123456).Returns(new PurchaseOrder
                                                                      {
                                                                          DocumentType = new DocumentType { Name = "RO" },
                                                                          Details = new List<PurchaseOrderDetail>
                                                                              {
                                                                                  new PurchaseOrderDetail { Line = 1 }
                                                                              }
                                                                      });
            this.action = () => this.Sut.UpdateDeliveriesForOrderLine(
                123456,
                1,
                new List<PurchaseOrderDelivery>(),
                new List<string>());
        }

        [Test]
        public void ShouldThrownException()
        {
            this.action.Should().Throw<PurchaseOrderDeliveryException>()
                .WithMessage("Cannot split deliveries - Order is not a PO.");
        }
    }
}
