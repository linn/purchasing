namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Email;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingPurchaseOrdersWithoutCopy : ContextBase
    {
        private ProcessResult result;

        private IList<int> orders;

        private int userNumber;

        private PurchaseOrder order123;

        [SetUp]
        public void SetUp()
        {
            this.orders = new List<int> { 123 };
            this.userNumber = 808;

            this.order123 = new PurchaseOrder
                                {
                                    OrderNumber = 123,
                                    AuthorisedById = 123,
                                    BaseOrderNetTotal = 123,
                                    Supplier = new Supplier
                                                   {
                                                       SupplierId = 777,
                                                       SupplierContacts =
                                                           new List<SupplierContact>
                                                               {
                                                                   new SupplierContact
                                                                       {
                                                                           IsMainOrderContact = "Y",
                                                                           EmailAddress = "email777"
                                                                       }
                                                               }
                                                   },
                                    Details = new List<PurchaseOrderDetail>
                                                  {
                                                      new PurchaseOrderDetail { PartNumber = "P1" }
                                                  }
                                };

            this.PurchaseOrderRepository.FindById(123).Returns(this.order123);
            this.MiniOrderRepository.FindById(123).Returns(new MiniOrder { OrderNumber = 123 });
            this.result = this.Sut.EmailMultiplePurchaseOrders(this.orders, this.userNumber, false);
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
            message.Should().Contain("Order 123 emailed successfully to email777");
        }

        [Test]
        public void ShouldNotGetEmployee()
        {
            this.EmployeeRepository.DidNotReceive().FindById(Arg.Any<int>());
        }

        [Test]
        public void ShouldSendEmail()
        {
            this.EmailService.Received().SendEmail(
                    "email777",
                    "email777",
                    Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                    Arg.Is<IEnumerable<Dictionary<string, string>>>(a => a.All(b => !b.ContainsValue("fred@co"))),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IEnumerable<Attachment>>(
                        a => a.First().FileName == $"LinnPurchaseOrder{this.order123.OrderNumber}.pdf"));
        }

        [Test]
        public void ShouldUpdateOrder()
        {
            this.order123.SentByMethod.Should().Be("EMAIL");
        }
    }
}
