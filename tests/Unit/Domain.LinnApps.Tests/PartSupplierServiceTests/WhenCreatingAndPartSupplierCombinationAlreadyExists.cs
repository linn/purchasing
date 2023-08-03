namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndPartSupplierCombinationAlreadyExists : ContextBase
    {
        private Action action;

        private PartSupplier candidate;

        [SetUp]
        public void SetUp()
        {
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
                                   Currency = new Currency { Code = "GBP" }
            };

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(this.candidate);

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierCreate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.CreatePartSupplier(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>();
        }

        [Test]
        public void ShouldReportPartAndSupplierCombinationExists()
        {
            this.action.Should().Throw<PartSupplierException>()
                .WithMessage(
                    "This part and supplier ID combination has already been created");
        }
    }
}
