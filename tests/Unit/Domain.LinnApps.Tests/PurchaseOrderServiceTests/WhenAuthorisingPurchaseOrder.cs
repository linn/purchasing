﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorisingPurchaseOrder : ContextBase
    {
        private ProcessResult result;

        private int userNumber;

        private PurchaseOrder order123;

        [SetUp]
        public void SetUp()
        {
            this.userNumber = 808;

            this.order123 = new PurchaseOrder
                              {
                                  OrderNumber = 123,
                                  AuthorisedById = null,
                                  BaseOrderNetTotal = 123,
                                  Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P1" } }
                              };
            this.PurchaseOrderRepository.FindById(123).Returns(this.order123);
            this.PurchaseOrderRepository.FindById(456).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 456,
                        AuthorisedById = null,
                        BaseOrderNetTotal = 12300,
                        Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P2" } }
                    });
            this.PurchaseOrderRepository.FindById(789).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 789,
                        AuthorisedById = 654321,
                        BaseOrderNetTotal = 123,
                        Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P3" } }
                    });
            this.PurchaseOrderRepository.FindById(101).Returns((PurchaseOrder)null);

            this.PurchaseOrdersPack.OrderCanBeAuthorisedBy(123, null, this.userNumber, null, null, null)
                .Returns(true);

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderAuthorise, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.AuthorisePurchaseOrder(this.order123, this.userNumber, new List<string>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }

        [Test]
        public void ShouldCheckForPermission()
        {
            this.MockAuthService.Received().HasPermissionFor(
                AuthorisedAction.PurchaseOrderAuthorise,
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnMessage()
        {
            var message = this.result.Message;
            message.Should().Be("Order 123 successfully authorised");
        }

        [Test]
        public void ShouldUpdateOrder()
        {
            this.order123.AuthorisedById.Should().Be(this.userNumber);
        }
    }
}
