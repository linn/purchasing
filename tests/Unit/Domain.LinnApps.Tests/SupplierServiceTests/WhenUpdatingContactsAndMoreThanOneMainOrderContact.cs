﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    using NUnit.Framework;

    public class WhenUpdatingContactsAndMoreThanOneMainOrderContact : AuthorisedContext
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
                                   Country = "GB",
                                   PaymentMethod = "PAYMENT METHOD",
                                   OrderAddress = new Address { FullAddress = new FullAddress { AddressString = "ADDRESS", Id = 1 } },
                                   AccountController = new Employee { Id = 123 },
                                   SupplierContacts = new List<SupplierContact>
                                                  {
                                                      new SupplierContact
                                                          {
                                                              ContactId = 1,
                                                              IsMainOrderContact = "Y",
                                                              IsMainInvoiceContact = "N",

                                                              SupplierId = 1,
                                                          },
                                                      new SupplierContact
                                                          {
                                                              ContactId = 2,
                                                              IsMainOrderContact = "Y",
                                                              IsMainInvoiceContact = "Y",
                                                              SupplierId = 1,
                                                          }
                                                  }
                               };

            this.action = () => this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<SupplierException>()
                .WithMessage("The inputs for the following fields are empty/invalid: Cannot have more than one Main Order Contact, ");
        }
    }
}
