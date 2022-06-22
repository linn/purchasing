namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingPdfEmail : ContextBase
    {
        private readonly int employeeNumber = 33107;

        private readonly int orderNumber = 5678;

        private readonly string supplierEmail = "seller@wesellthings.com";

        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.EmployeeRepository.FindById(this.employeeNumber).Returns(
                new Employee
                    {
                        Id = this.employeeNumber,
                        FullName = "mario",
                        PhoneListEntry = new PhoneListEntry { EmailAddress = "mario@karting.com" }
                    });

            this.EmailService.SendEmail(
                this.supplierEmail,
                this.supplierEmail,
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                "purchasingoutgoing@linn.co.uk",
                "Linn Purchasing",
                $"Linn Purchase Order {this.orderNumber}",
                Arg.Any<string>(),
                Arg.Any<Stream>(),
                $"LinnPurchaseOrder{this.orderNumber}");

            this.result = this.Sut.SendPdfEmail(
                "<h1>hello world order number is @Model.OrderNumber</h1>",
                "seller@wesellthings.com",
                this.orderNumber,
                true,
                this.employeeNumber);
        }

        [Test]
        public void ShouldCallSendEmail()
        {
            this.EmailService.Received().SendEmail(
                this.supplierEmail,
                this.supplierEmail,
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                "purchasingoutgoing@linn.co.uk",
                "Linn Purchasing",
                $"Linn Purchase Order {this.orderNumber}",
                Arg.Any<string>(),
                Arg.Any<Stream>(),
                $"LinnPurchaseOrder{this.orderNumber}");
        }

        [Test]
        public void ShouldReturnSuccessProcessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Email Sent");
        }
    }
}
