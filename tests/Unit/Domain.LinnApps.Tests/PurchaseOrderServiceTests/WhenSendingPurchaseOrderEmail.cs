﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Email;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingPurchaseOrderEmail : ContextBase
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

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber)
                .Returns(new PurchaseOrder 
                             {
                                 OrderNumber = this.orderNumber, 
                                 AuthorisedById = 100,
                                 DocumentType = new DocumentType { Name = "PO" }
                             });
            this.result = this.Sut.SendPdfEmail("seller@wesellthings.com", this.orderNumber, true, this.employeeNumber);
        }

        [Test]
        public void ShouldCallSendEmailWithOneAttachment()
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
                Arg.Is<IEnumerable<Attachment>>(a => a.Count() == 1 && a.First().FileName ==
                                                     $"LinnPurchaseOrder{this.orderNumber}.pdf"));
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
