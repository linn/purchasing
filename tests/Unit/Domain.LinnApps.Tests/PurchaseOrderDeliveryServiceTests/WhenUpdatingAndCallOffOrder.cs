namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndCallOffOrder : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PurchaseOrderRepository.FindById(1).Returns(new PurchaseOrder
                                                                 {
                                                                     OrderNumber = 1,
                                                                     OrderMethod = new OrderMethod
                                                                         {
                                                                             Name = "CALL OFF"
                                                                         },
                                                                     Details = new List<PurchaseOrderDetail>
                                                                                   {
                                                                                       new PurchaseOrderDetail
                                                                                           {
                                                                                               Line = 1
                                                                                           }
                                                                                   }
                                                                 });
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
                .WithMessage("You cannot raise a split delivery for a CALL OFF. It is raised automatically on delivery.");
        }
    }
}
