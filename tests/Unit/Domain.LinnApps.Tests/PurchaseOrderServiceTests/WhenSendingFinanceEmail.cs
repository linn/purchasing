namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingFinanceEmail : ContextBase
    {
        private readonly int employeeNumber = 33107;

        private readonly int orderNumber = 5678;

        private readonly string supplierEmail = "seller@wesellthings.com";

        private MiniOrder miniOrder;

        private ProcessResult result;

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


            this.EmployeeRepository.FindById(32864).Returns(
                new Employee
                    {
                        Id = 32864,
                        FullName = "scary finance boss 1",
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "luigi@karting.com" }
                    });

            this.EmployeeRepository.FindById(32835).Returns(
                new Employee
                    {
                        Id = 32835,
                        FullName = "scary finance boss 2",
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "bowser@karting.com" }
                    });

            this.PurchaseOrderRepository.FindById(this.orderNumber).Returns(
                new PurchaseOrder { OrderNumber = this.orderNumber, Supplier = new Supplier { Name = "quick parts" } });
            this.result = this.Sut.SendFinanceAuthRequestEmail(this.employeeNumber, this.orderNumber);
        }

        [Test]
        public void ShouldCallSendEmail()
        {
            this.EmailService.Received().SendEmail(
                "luigi@karting.com",
                "scary finance boss 1",
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<string>(),
                "Linn Purchasing",
                $"Purchase Order {this.orderNumber} requires Authorisation",
                Arg.Any<string>(),
                null,
                null);
        }

        [Test]
        public void ShouldReturnSuccessProcessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Email sent for purchase order 5678 auth request to Finance");
        }
    }
}
