namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingDept : ContextBase
    {
        private ProcessResult result;

        private PurchaseOrder order;

        private Employee user;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {
            this.user = new Employee
                            {
                                Id = 123,
                                PhoneListEntry = new PhoneListEntry()
                                                     {
                                                         EmailAddress = "test@user.com"
                                                     },
                                FullName = "USER MCUSERSON"
                            };
            this.supplier = new Supplier { Name = "SUPPLIER" };
            this.order = new PurchaseOrder
                             {
                                 OrderNumber = 321,
                                 AuthorisedById = 456,
                                 Supplier = this.supplier,
                                 RequestedBy = new Employee
                                                 {
                                                     FullName = "PERSON",
                                                     PhoneListEntry 
                                                         = new PhoneListEntry
                                                               {
                                                                   EmailAddress = "purchasing@shop.com"
                                                               }
                                                 }
                             };

            this.PurchaseOrderRepository.FindById(this.order.OrderNumber).Returns(this.order);
            this.EmployeeRepository.FindById(this.user.Id).Returns(this.user);

            this.result = this.Sut.EmailDept(this.order.OrderNumber, this.user.Id);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }

        [Test]
        public void ShouldSendEmail()
        {
            var expectedBody =
                "Please click the link when you have received the goods against this order to confirm delivery. \n";
            expectedBody += "This will also confirm that payment can be made.  \n";
            expectedBody += $"http://app.linn.co.uk/purch/po/podelcon.aspx?po={this.order.OrderNumber}  \n";
            expectedBody += "Any queries regarding this order - please contact a member of the Finance team.";
            
            this.EmailService.Received().SendEmail(
                this.order.RequestedBy.PhoneListEntry.EmailAddress,
                this.order.RequestedBy.FullName,
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                this.user.PhoneListEntry.EmailAddress,
                this.user.FullName,
                $"Purchase Order {this.order.OrderNumber} for Supplier {this.order.Supplier.Name}",
                expectedBody);
        }
    }
}
