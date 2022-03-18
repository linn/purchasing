namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingContact : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private SupplierContact currentSupplierContact;

        private SupplierContact updatedSupplierContact;

        private Person updatedPerson;

        [SetUp]
        public void SetUp()
        {
            this.currentSupplierContact = new SupplierContact { SupplierId = 1, ContactId = 1 };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.updatedPerson = new Person { FirstName = "A", LastName = "CONTACT", Id = 123 };

            this.updatedSupplierContact = new SupplierContact
                                       {
                                           ContactId = 1,
                                           IsMainInvoiceContact = "Y",
                                           IsMainOrderContact = "N",
                                           SupplierId = 1,
                                           Person = this.updatedPerson
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
                OrderAddress = new Address { FullAddress = new FullAddress { AddressString = "ADDRESS", Id = 1 } },
                AccountController = new Employee { Id = 123 },
                SupplierContacts = new List<SupplierContact>
                               {
                                   this.updatedSupplierContact
                               }
            };
            this.SupplierContactRepository.FindById(this.updatedSupplierContact.ContactId).Returns(this.currentSupplierContact);
            this.SupplierContactRepository.FindById(this.updatedSupplierContact.ContactId).Returns(this.updatedSupplierContact);
            this.PersonRepository.FindById(this.updatedPerson.Id).Returns(this.updatedPerson);
            this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdateContactAndPerson()
        {
            this.current.SupplierContacts.Count().Should().Be(1);
            var first = this.current.SupplierContacts.First();
            first.IsMainInvoiceContact.Should().Be(this.updatedSupplierContact.IsMainInvoiceContact);
            first.IsMainOrderContact.Should().Be(this.updatedSupplierContact.IsMainOrderContact);
            first.EmailAddress.Should().Be(this.updatedSupplierContact.EmailAddress);
            first.PhoneNumber.Should().Be(this.updatedSupplierContact.PhoneNumber);
            first.Person.LastName.Should().Be(this.updatedPerson.LastName);
            first.Person.FirstName.Should().Be(this.updatedPerson.FirstName);
        }
    }
}
