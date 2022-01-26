namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenTryingToSetPreferredSupplierThatIsAlreadyPreferredForAPart : ContextBase
    {
        private Action action;

        private PreferredSupplierChange candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     NewSupplier = new Supplier { SupplierId = 1 },
                                     OldSupplier = new Supplier { SupplierId = 1 }
                                 };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { BomType = "C" });
            this.action = () => this.Sut.CreatePreferredSupplierChange(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>().WithMessage(
                "Selected  supplier is already the preferred supplier for this part.");
        }
    }
}
