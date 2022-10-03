namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Email;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingCreditOrderEmail : ContextBase
    {
        private readonly int employeeNumber = 33107;

        private readonly int orderNumber = 5678;

        private readonly string supplierEmail = "seller@wesellthings.com";

        private MiniOrder miniOrder;

        private ProcessResult result;

        private PurchaseOrder order;

        [SetUp]
        public void SetUp()
        {
            this.miniOrder = new MiniOrder { OrderNumber = this.orderNumber };
            this.order = new PurchaseOrder
            {
                OrderNumber = this.orderNumber,
                DocumentType = new DocumentType { Name = "CO" }
            };
            this.EmployeeRepository.FindById(this.employeeNumber).Returns(
                new Employee
                {
                    Id = this.employeeNumber,
                    FullName = "mario",
                    PhoneListEntry = new PhoneListEntry { EmailAddress = "mario@karting.com" }
                });

            this.NoteRepository.FindBy(Arg.Any<Expression<Func<PlCreditDebitNote, bool>>>())
                .Returns(new PlCreditDebitNote
                {
                    PurchaseOrder = this.order
                });

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber)
                .Returns(this.order);
            this.result = this.Sut.SendPdfEmail("seller@wesellthings.com", this.orderNumber, true, this.employeeNumber);
        }

        [Test]
        public void ShouldCallSendEmailWithAttachments()
        {
            this.EmailService.Received().SendEmail(
                this.supplierEmail,
                this.supplierEmail,
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<string>(),
                "Linn Purchasing",
                $"Linn Purchase Order {this.orderNumber}",
                Arg.Any<string>(),
                Arg.Is<IEnumerable<Attachment>>(a => a.Count() == 2 &&
                                                     a.First().FileName ==
                                                     $"LinnPurchaseOrder{this.orderNumber}.pdf" &&
                                                     a.ElementAt(1).FileName ==
                                                     $"DebitNote.pdf"));
        }

        [Test]
        public void ShouldReturnSuccessProcessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("Email sent for purchase order 5678 to seller@wesellthings.com");
        }

        [Test]
        public void ShouldSetSentByMethod()
        {
            this.miniOrder.SentByMethod.Should().Be("EMAIL");
        }
    }
}
