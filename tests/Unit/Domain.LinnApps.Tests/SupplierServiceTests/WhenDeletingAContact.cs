namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDeletingAContact : AuthorisedContext
    {
        private SupplierContact toDelete;

        private Supplier currentSupplier;

        private Supplier updatedSupplier;

        [SetUp]
        public void Setup()
        {
            this.toDelete = new SupplierContact { ContactId = 123, Person = new Person() };

            this.currentSupplier = new Supplier
                                       {
                                           SupplierId = 666,
                                           SupplierContacts = new SupplierContact[]
                                                                  {
                                                                      new SupplierContact { IsMainInvoiceContact = "Y" },
                                                                      new SupplierContact { IsMainOrderContact = "Y" },
                                                                      this.toDelete
                                                                  }
                                       };

            this.updatedSupplier = new Supplier
                                       {
                                           SupplierId = 666,
                                           Name = "SUPP",
                                           InvoiceContactMethod = "EMAIL",
                                           PaymentDays = 14, 
                                           PaymentMethod = "BACS",
                                           Country = "GB",
                                           AccountController = new Employee(),
                                           OrderAddress = new Address(),
                                           SupplierContacts = new SupplierContact[]
                                                                  {
                                                                      new SupplierContact
                                                                          {
                                                                              IsMainInvoiceContact = "Y", Person = new Person()
                                                                          },
                                                                      new SupplierContact
                                                                          {
                                                                              IsMainOrderContact = "Y", Person = new Person()
                                                                          }
                                                                  }
                                       };
            this.PersonRepository.FindById(Arg.Any<int>()).Returns(new Person());
            this.SupplierContactRepository.FilterBy(Arg.Any<Expression<Func<SupplierContact, bool>>>())
                .Returns(this.currentSupplier.SupplierContacts.AsQueryable());
            this.SupplierContactRepository.FindById(this.toDelete.ContactId).Returns(this.toDelete);
            this.Sut.UpdateSupplier(
                this.currentSupplier, this.updatedSupplier, new List<string>());
        }

        [Test]
        public void ShouldMarkDeletedRecordAsInvalid()
        {
            this.toDelete.DateInvalid.Should().NotBeNull();
        }
    }
}
