namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingContactThatAlreadyExists : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Contact updatedContact;

        private SupplierContact currentSupplierContact;

        private SupplierContact updateSupplierContact;

        private Person updatedPerson;

        [SetUp]
        public void SetUp()
        {
            this.currentSupplierContact = new SupplierContact { SupplierId = 1, ContactId = 1 };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.updatedPerson = new Person { FirstName = "A", LastName = "CONTACT", Id = 123 };

            this.updatedContact = new Contact
                                      {
                                          ContactId = 1, 
                                          PhoneNumber = "123",
                                          EmailAddress = "email@address.com",
                                          Person = this.updatedPerson
                                      };

            this.updateSupplierContact = new SupplierContact
                                       {
                                           ContactId = 1,
                                           IsMainInvoiceContact = "Y",
                                           IsMainOrderContact = "N",
                                           SupplierId = 1,
                                           Contact = this.updatedContact
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
                Contacts = new List<SupplierContact>
                               {
                                   this.updateSupplierContact
                               }
            };
            this.SupplierContactRepository.FindById(this.updatedContact.ContactId).Returns(this.currentSupplierContact);
            this.ContactRepository.FindById(this.updatedContact.ContactId).Returns(this.updatedContact);
            this.PersonRepository.FindById(this.updatedPerson.Id).Returns(this.updatedPerson);
            this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdateContactAndPerson()
        {
            this.current.Contacts.Count().Should().Be(1);
            var first = this.current.Contacts.First();
            first.IsMainInvoiceContact.Should().Be(this.updateSupplierContact.IsMainInvoiceContact);
            first.IsMainOrderContact.Should().Be(this.updateSupplierContact.IsMainOrderContact);
            first.Contact.EmailAddress.Should().Be(this.updatedContact.EmailAddress);
            first.Contact.PhoneNumber.Should().Be(this.updatedContact.PhoneNumber);
            first.Contact.Person.LastName.Should().Be(this.updatedPerson.LastName);
            first.Contact.Person.FirstName.Should().Be(this.updatedPerson.FirstName);
        }
    }
}
