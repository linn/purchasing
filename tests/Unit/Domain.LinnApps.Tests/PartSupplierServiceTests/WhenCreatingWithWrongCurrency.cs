namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingWithWrongCurrency : ContextBase
    {
        private PartSupplier candidate;

        private Currency candidateCurrency;

        private Action action;

        private Currency supplierCurrency;

        [SetUp]
        public void SetUp()
        {
            this.candidateCurrency = new Currency { Code = "GBP" };
            this.supplierCurrency = new Currency { Code = "USD" };
            this.candidate = new PartSupplier
                                 {
                                     PartNumber = "PART", 
                                     SupplierId = 1, 
                                     MinimumOrderQty = 10,
                                     CreatedBy = new Employee { Id = 33087 },
                                     OrderIncrement = 1m,
                                     LeadTimeWeeks = 1,
                                     DateCreated = DateTime.UnixEpoch,
                                     SupplierDesignation = "1234567",
                                     CurrencyUnitPrice = 1m,
                                     DamagesPercent = 0m,
                                     MinimumDeliveryQty = 1m,
                                     OrderMethod = new OrderMethod { Name = "METHOD" },
                                     Part = new Part { PartNumber = "PART" },
                                     Currency = this.candidateCurrency
            };

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "PART", Description = "DESC" });
            this.SupplierRepository.FindById(1)
                .Returns(new Supplier { SupplierId = 1, Name = "A SUPPLIER", Currency = this.supplierCurrency });
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PartSupplierCreate, 
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.CreatePartSupplier(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should()
                .Throw<PartSupplierException>()
                .WithMessage("Supplier 1 has currency USD. Cannot create a part supplier with currency GBP.");
        }
    }
}
