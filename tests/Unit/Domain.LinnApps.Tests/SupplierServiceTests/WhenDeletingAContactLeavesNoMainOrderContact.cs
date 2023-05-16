namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDeletingAContactLeavesNoMainOrderContact : AuthorisedContext
    {
        private SupplierContact toDelete;

        private Supplier currentSupplier;

        private Supplier updatedSupplier;

        private Action action;

        [SetUp]
        public void Setup()
        {
            this.toDelete = new SupplierContact { ContactId = 123, Person = new Person(), IsMainOrderContact = "Y" };

            this.currentSupplier = new Supplier
            {
                SupplierId = 666,
                SupplierContacts = new SupplierContact[]
                                       {
                                           new SupplierContact { IsMainInvoiceContact = "Y" },
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
                AccountController = new Employee(),
                OrderAddress = new Address(),
                SupplierContacts = new SupplierContact[]
                                       {
                                           new SupplierContact
                                               {
                                                   IsMainInvoiceContact = "Y", Person = new Person()
                                               }
                                       }
            };
            this.PersonRepository.FindById(Arg.Any<int>()).Returns(new Person());
            this.SupplierContactRepository.FilterBy(Arg.Any<Expression<Func<SupplierContact, bool>>>())
                .Returns(this.currentSupplier.SupplierContacts.AsQueryable());
            this.SupplierContactRepository.FindById(this.toDelete.ContactId).Returns(this.toDelete);
            this.action = () => this.Sut.UpdateSupplier(
                this.currentSupplier, this.updatedSupplier, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<SupplierException>()
                .WithMessage("The inputs for the following fields are empty/invalid: Main Order Contact, ");
        }
    }
}
