namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorisingPurchaseOrders : ContextBase
    {
        private ProcessResult result;

        private IList<int> orders;

        private int userNumber;

        private PurchaseOrder order123;

        [SetUp]
        public void SetUp()
        {
            this.orders = new List<int> { 123, 456, 789 };
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

            this.PurchaseOrdersPack.OrderCanBeAuthorisedBy(123, null, this.userNumber, null, null, null)
                .Returns(true);
            this.PurchaseOrdersPack.OrderCanBeAuthorisedBy(456, null, this.userNumber, null, null, null)
                .Returns(false);

            this.result = this.Sut.AuthoriseMultiplePurchaseOrders(this.orders, this.userNumber);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnMessage()
        {
            var message = this.result.Message;
            message.Should().Contain("Order 123 authorised successfully");
            message.Should().Contain("Order 456 YOU CANNOT AUTHORISE THIS ORDER");
            message.Should().Contain("Order 789 was already authorised");
            message.Should().Contain("1 out of 3 authorised successfully");
        }

        [Test]
        public void ShouldUpdateOrder()
        {
            this.order123.AuthorisedById.Should().Be(this.userNumber);
        }
    }
}
