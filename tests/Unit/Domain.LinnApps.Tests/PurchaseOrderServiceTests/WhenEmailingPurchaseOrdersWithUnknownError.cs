namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Email;
    using Linn.Common.Logging;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingPurchaseOrdersWithUnknownError : ContextBase
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

            this.PurchaseOrderRepository.FindById(101)
                .Returns((PurchaseOrder)null);
            this.EmployeeRepository.FindById(this.userNumber)
                .Returns(new Employee { FullName = "Fred", PhoneListEntry = new PhoneListEntry { EmailAddress = "fred@co" } });
            this.MiniOrderRepository.FindById(123).Returns(new MiniOrder { OrderNumber = 123 });

            this.EmailService.When(a => a.SendEmail(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IEnumerable<Attachment>>())).Do(
                b => throw new Exception("Some unknown error"));

            this.result = this.Sut.EmailMultiplePurchaseOrders(this.orders, this.userNumber, true);
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
            message.Should().Contain("Order 123 to email777 failed with unknown exception. Some unknown error. ");
        }

        [Test]
        public void ShouldLogError()
        {
            this.Log.Received().Warning("Order 123 to email777 failed with unknown exception. Some unknown error.");
        }

        [Test]
        public void ShouldSendEmail()
        {
            this.EmailService.Received(1).SendEmail(
                    "email777",
                    "email777",
                    Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                    Arg.Is<IEnumerable<Dictionary<string, string>>>(a => a.Any(b => b.ContainsValue("fred@co"))),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IEnumerable<Attachment>>(a => a.All(b => b.Type == "pdf")));
        }

        [Test]
        public void ShouldNotUpdateOrder()
        {
            this.order123.SentByMethod.Should().BeNull();
        }
    }
}
