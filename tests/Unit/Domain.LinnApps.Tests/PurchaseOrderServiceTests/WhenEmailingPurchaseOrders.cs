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

    public class WhenEmailingPurchaseOrders : ContextBase
    {
        private ProcessResult result;

        private IList<int> orders;

        private int userNumber;

        private PurchaseOrder order123;

        [SetUp]
        public void SetUp()
        {
            this.orders = new List<int> { 123, 456, 789, 101, 202 };
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
            this.PurchaseOrderRepository.FindById(456).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 456,
                        AuthorisedById = 123,
                        Cancelled = "Y",
                        BaseOrderNetTotal = 12300,
                        Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P2" } }
                    });
            this.PurchaseOrderRepository.FindById(789).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 789,
                        AuthorisedById = null,
                        BaseOrderNetTotal = 123,
                        Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P3" } }
                    });
            this.PurchaseOrderRepository.FindById(202).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 202,
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
                                                           }
                                                   }
                                       },
                    Details = new List<PurchaseOrderDetail> { new PurchaseOrderDetail { PartNumber = "P3" } }
                    });
            this.PurchaseOrderRepository.FindById(101)
                .Returns((PurchaseOrder)null);
            this.EmployeeRepository.FindById(this.userNumber)
                .Returns(new Employee { FullName = "Fred", PhoneListEntry = new PhoneListEntry { EmailAddress = "fred@co" } });
            this.MiniOrderRepository.FindById(123).Returns(new MiniOrder { OrderNumber = 123 });
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
            message.Should().Contain("Order 123 emailed successfully to email777");
            message.Should().Contain("Order 456 is cancelled");
            message.Should().Contain("Order 789 is not authorised");
            message.Should().Contain("Order 101 could not be found");
            message.Should().Contain("Order 202 could not find order contact email");
            message.Should().Contain("1 out of 5 emailed successfully");
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
        public void ShouldUpdateOrder()
        {
            this.order123.SentByMethod.Should().Be("EMAIL");
        }
    }
}
