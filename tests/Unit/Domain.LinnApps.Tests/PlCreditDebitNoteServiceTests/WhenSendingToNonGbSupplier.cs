﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNoteServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingToNonGbSupplier : ContextBase
    {
        private ProcessResult result;

        private Supplier supplier;

        private Employee financePerson;

        private Employee sender;

        private PlCreditDebitNote note;

        [SetUp]
        public void SetUp()
        {
            this.sender = new Employee
            {
                Id = 1,
                PhoneListEntry = new PhoneListEntry { EmailAddress = "employee@linn.com" },
                FullName = "Purchasing Employee"
            };
            this.financePerson = new Employee
            {
                Id = 6001,
                PhoneListEntry = new PhoneListEntry { EmailAddress = "nongbperson@linn.co.uk" },
                FullName = "NON GB PERSON"
            };
            this.supplier = new Supplier
            {
                Country = "NOT GB",
                SupplierContacts = new List<SupplierContact>
                                                             {
                                                                 new SupplierContact
                                                                     {
                                                                         EmailAddress = "supplier@address.com",
                                                                         IsMainOrderContact = "Y",
                                                                         Person = new Person
                                                                             {
                                                                                 FirstName = "MR", LastName = "SUPPLIER"
                                                                             }
                                                                     }
                                                             }
            };

            this.note = new PlCreditDebitNote
            {
                Supplier = this.supplier,
                NoteNumber = 666,
                NoteType = new CreditDebitNoteType { PrintDescription = "CREDIT NOTE" }
            };

            this.MockEmployeeRepository.FindById(this.financePerson.Id).Returns(this.financePerson);
            this.MockEmployeeRepository.FindById(this.sender.Id).Returns(this.sender);
            this.result = this.Sut.SendEmails(
                this.sender,
                this.note,
                null);
        }

        [Test]
        public void ShouldLookUpCorrectBccPerson()
        {
            this.MockEmployeeRepository.Received().FindById(this.financePerson.Id);
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().Be(true);
            this.result.Message.Should().Be("Email Sent");
        }
    }
}
