namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingNewContact : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private SupplierContact newSupplierContact;

        private Person newPerson;

        [SetUp]
        public void SetUp()
        {
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.newPerson = new Person { FirstName = "A", LastName = "CONTACT", Id = -1 };

            this.newSupplierContact = new SupplierContact
            {
                ContactId = 1,
                IsMainInvoiceContact = "Y",
                IsMainOrderContact = "N",
                SupplierId = 1,
                Person = this.newPerson
            };

            this.updated = new Supplier
            {
                Name = "NEW NAME",
                SupplierId = 1,
                VendorManager = new VendorManager { Id = "V" },
                InvoiceContactMethod = "POST",
                OrderContactMethod = "EMAIL",
                PaymentDays = 1,
                PaymentMethod = "PAYMENT METHOD",
                OrderFullAddress = new FullAddress { AddressString = "ADDRESS", Id = 1 },
                AccountController = new Employee { Id = 123 },
                SupplierContacts = new List<SupplierContact>
                               {
                                   this.newSupplierContact
                               }
            };
            this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }


        [Test]
        public void ShouldAddPerson()
        {
            this.PersonRepository.Received()
                .Add(Arg.Is<Person>(p => p.FirstName == this.newPerson.FirstName
                                         && p.LastName == this.newPerson.LastName));
        }

        [Test]
        public void ShouldAddSupplierContact()
        {
            this.SupplierContactRepository.Received()
                .Add(Arg.Is<SupplierContact>(p => 
                    p.EmailAddress == this.newSupplierContact.EmailAddress
                    && p.PhoneNumber == this.newSupplierContact.PhoneNumber
                    && p.Comments == this.newSupplierContact.Comments
                    && p.JobTitle == this.newSupplierContact.JobTitle));
        }

        [Test]
        public void ShouldUpdateContactAndPerson()
        {
            this.current.SupplierContacts.Count().Should().Be(1);
            var first = this.current.SupplierContacts.First();
            first.IsMainInvoiceContact.Should().Be(this.newSupplierContact.IsMainInvoiceContact);
            first.IsMainOrderContact.Should().Be(this.newSupplierContact.IsMainOrderContact);
            first.EmailAddress.Should().Be(this.newSupplierContact.EmailAddress);
            first.PhoneNumber.Should().Be(this.newSupplierContact.PhoneNumber);
            first.Person.LastName.Should().Be(this.newPerson.LastName);
            first.Person.FirstName.Should().Be(this.newPerson.FirstName);
            first.JobTitle.Should().Be(this.newSupplierContact.JobTitle);
            first.JobTitle.Should().Be(this.newSupplierContact.Comments);
        }
    }
}
