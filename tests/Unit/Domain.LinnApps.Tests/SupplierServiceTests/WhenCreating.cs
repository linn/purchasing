namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : AuthorisedContext
    {
        private Supplier result;

        private Supplier candidate;

        private IEnumerable<string> privileges;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new Supplier
                                 {
                                     SupplierId = 1, 
                                     Name = "SUPPLIER",
                                     InvoiceContactMethod = "METHOD",
                                     PaymentDays = 1,
                                     PaymentMethod = "PAYMENT METHOD",
                                     AccountController = new Employee { Id = 1 },
                                     OrderFullAddress = new FullAddress { Id = 1 }
                                 };

            this.EmployeeRepository.FindById(1).Returns(new Employee { Id = 1 });
            this.MockAddressRepository.FindById(1).Returns(new FullAddress { Id = 1 });
            this.privileges = new List<string> { "priv" };
            this.result = this.Sut.CreateSupplier(this.candidate, this.privileges);
        }

        [Test]
        public void ShouldReturnResult()
        {
            this.result.SupplierId.Should().Be(1);
            this.result.Name.Should().Be("SUPPLIER");
        }
    }
}
