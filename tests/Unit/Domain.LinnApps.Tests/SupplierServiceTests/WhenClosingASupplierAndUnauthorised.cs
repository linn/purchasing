namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenClosingASupplierAndUnauthorised : UnauthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Currency currency;

        private Action action;

        private Address address;

        private IEnumerable<string> privileges;

        [SetUp]
        public void SetUp()
        {
            this.currency = new Currency { Code = "USD" };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.address = new Address { FullAddress = new FullAddress { AddressString = "ADDRESS", Id = 1 } };


            this.updated = new Supplier
            {
                Name = "Supplier",
                SupplierId = 1,
                Currency = this.currency,
                VendorManager = new VendorManager { Id = "V" },
                InvoiceContactMethod = "POST",
                LiveOnOracle = "Y",
                OrderContactMethod = "EMAIL",
                Planner = new Planner { Id = 1 },
                PaymentDays = 1,
                PaymentMethod = "PAYMENT METHOD",
                AccountingCompany = "LINN",
                OrderHold = "Y",
                OrderAddress = this.address,
                AccountController = new Employee { Id = 123 },
                ReasonClosed = "SHUT DOWN",
                ClosedBy = new Employee { Id = 33087 }
            };

            this.MockCurrencyRepository
                .FindById(this.updated.Currency.Code).Returns(this.currency);
            this.MockAddressRepository.FindById(1).Returns(this.address);
            this.privileges = new List<string> { "priv" };
            this.VendorManagerRepository.FindById("V").Returns(new VendorManager { Id = "V" });
            this.EmployeeRepository.FindById(33087).Returns(new Employee { Id = 33087 });
            this.action = () => this.Sut.UpdateSupplier(this.current, this.updated, this.privileges);
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>().WithMessage("You are not authorised to close a supplier");
        }
    }
}
