namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingPurchaseOrderEmailButUnauthorised : ContextBase
    {
        private readonly int employeeNumber = 33107;

        private readonly int orderNumber = 5678;

        private MiniOrder miniOrder;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.miniOrder = new MiniOrder { OrderNumber = this.orderNumber };

            this.EmployeeRepository.FindById(this.employeeNumber).Returns(
                new Employee
                    {
                        Id = this.employeeNumber,
                        FullName = "mario",
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "mario@karting.com" }
                    });

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber)
                .Returns(new PurchaseOrder 
                             { 
                                 OrderNumber = this.orderNumber, 
                                 DocumentType = new DocumentType { Name = "PO" }
                             });
            this.action = () => this.Sut.SendPdfEmail(
                "seller@wesellthings.com",
                this.orderNumber,
                true,
                this.employeeNumber);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<UnauthorisedOrderException>();
        }
    }
}
