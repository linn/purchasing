namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingPurchaseOrderEmailNoEmailConfigured : ContextBase
    {
        private readonly int employeeNumber = 33107;

        private readonly int orderNumber = 5678;

        private MiniOrder miniOrder;

        private Employee employee;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.miniOrder = new MiniOrder { OrderNumber = this.orderNumber };

            this.employee = new Employee
            {
                Id = this.employeeNumber,
                FullName = "mario",
            };

            this.EmployeeRepository.FindById(this.employeeNumber).Returns(
                this.employee);

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber)
                .Returns(new PurchaseOrder
                {
                    OrderNumber = this.orderNumber,
                    AuthorisedById = 100,
                    DocumentType = new DocumentType { Name = "PO" }
                });
            this.action = () => this.Sut.SendPdfEmail("seller@wesellthings.com", this.orderNumber, true, this.employeeNumber);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<PurchaseOrderException>().WithMessage("Cannot find sender Email address. Check phone list entry.");
        }
    }
}
