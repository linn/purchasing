namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPreferredSupplierChangeAndChosenSupplierHasNoPlannerOrVendorManager : ContextBase
    {
        private Action action;

        private PreferredSupplierChange candidate;

        private Supplier newSupplier;

        [SetUp]
        public void SetUp()
        {
            this.newSupplier = new Supplier { SupplierId = 1 };

            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     NewSupplier = this.newSupplier,
                                     OldSupplier = new Supplier { SupplierId = 2 }
                                 };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { BomType = "C" });
            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(new PartSupplier
                {
                    Supplier = this.newSupplier
                });
            this.action = () => this.Sut.CreatePreferredSupplierChange(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>().WithMessage(
                "Selected supplier is missing planner or vendor manager");
        }
    }
}
