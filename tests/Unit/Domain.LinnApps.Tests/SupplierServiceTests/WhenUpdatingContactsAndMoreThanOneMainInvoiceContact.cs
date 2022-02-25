namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    using NUnit.Framework;

    public class WhenUpdatingContactsAndMoreThanOneMainInvoiceContact : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };
            this.updated = new Supplier
            {
                Name = "NEW NAME",
                SupplierId = 1,
                VendorManager = new VendorManager { Id = "V" },
                InvoiceContactMethod = "POST",
                OrderContactMethod = "EMAIL",
                PaymentDays = 1,
                PaymentMethod = "PAYMENT METHOD",
                OrderFullAddress = new FullAddress { Id = 1 },
                InvoiceFullAddress = new FullAddress { AddressString = "ADDRESS", Id = 1 },
                AccountController = new Employee { Id = 123 },
                Contacts = new List<SupplierContact>
                                                  {
                                                      new SupplierContact
                                                          {
                                                              ContactId = 1,
                                                              IsMainInvoiceContact = "Y",
                                                              SupplierId = 1,
                                                          },
                                                      new SupplierContact
                                                          {
                                                              ContactId = 2,
                                                              IsMainInvoiceContact = "Y",
                                                              SupplierId = 1,
                                                          }
                                                  }
            };

            this.action = () => this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<SupplierException>()
                .WithMessage("Cannot have more than one Main Invoice Contact");
        }
    }
}
