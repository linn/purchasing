namespace Linn.Purchasing.Domain.LinnApps.Tests.PlCreditDebitNotesTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingToGbSupplier : ContextBase
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
                                          Id = 33039,
                                          PhoneListEntry = new PhoneListEntry { EmailAddress = "gbperson@linn.co.uk" },
                                          FullName = "GB PERSON"
                                      };
            this.supplier = new Supplier
                                  {
                                      Country = "GB",
                                      SupplierContacts = new List<SupplierContact>
                                                             {
                                                                 new SupplierContact
                                                                     {
                                                                         EmailAddress = "supplier@address.com",
                                                                         IsMainOrderContact = "Y",
                                                                         Person = new Person { FirstName = "MR", LastName = "SUPPLIER"}
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
            this.MockEmployeeRepository.Received().FindById(33039);
        }

        [Test]
        public void ShouldCallEmailServiceWithCorrectParameters()
        {
           this.MockEmailService.Received().SendEmail(
               this.supplier.SupplierContacts.First(x => x.IsMainOrderContact == "Y").EmailAddress,
               "MR SUPPLIER",
               Arg.Any<IEnumerable<Dictionary<string, string>>>(),
               Arg.Is<IEnumerable<Dictionary<string, string>>>(
                   l => l.FirstOrDefault(x => x.ContainsValue(this.financePerson.PhoneListEntry.EmailAddress)) != null),
               this.sender.PhoneListEntry.EmailAddress,
               this.sender.FullName,
               $"Linn Products {this.note.NoteType.PrintDescription} {this.note.NoteNumber}",
               string.Empty,
               null,
               $"{this.note.NoteType.PrintDescription} {this.note.NoteNumber}");
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().Be(true);
            this.result.Message.Should().Be("Email Sent");
        }
    }
}
