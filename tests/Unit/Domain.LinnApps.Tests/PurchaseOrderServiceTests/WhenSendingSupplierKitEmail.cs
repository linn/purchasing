namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingSupplierKitEmail : ContextBase
    {
        private ProcessResult result;

        private int orderNumber;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 123;
            this.SupplierKitService.GetSupplierKits(Arg.Is<PurchaseOrder>(a => a.OrderNumber == this.orderNumber), true)
                .Returns(
                    new List<SupplierKit>
                        {
                            new SupplierKit(new Part { PartNumber = "P1" }, 12m) { Details = new List<SupplierKitDetail>() }
                        });
            this.PurchaseOrderRepository.FindById(this.orderNumber).Returns(
                new PurchaseOrder { OrderNumber = this.orderNumber, Supplier = new Supplier { Name = "S1" } });
            this.result = this.Sut.SendSupplierAssemblyEmail(this.orderNumber);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnMessage()
        {
            this.result.Message.Should().Be("Email sent for purchase order 123 to Logistics");
        }

        [Test]
        public void ShouldCallSupplierKitService()
        {
            this.SupplierKitService.Received()
                .GetSupplierKits(Arg.Is<PurchaseOrder>(a => a.OrderNumber == this.orderNumber), true);
        }

        [Test]
        public void ShouldSendEmail()
        {
            this.EmailService.Received().SendEmail(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Test]
        public void ShouldGetOrder()
        {
            this.PurchaseOrderRepository.Received().FindById(this.orderNumber);
        }
    }
}
