namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomTypeWithNewSupplierNotOnPart : ContextBase
    {
        private Action action;

        private BomTypeChange bomTypeChange;

        [SetUp]
        public void SetUp()
        {
            this.authService
                .HasPermissionFor(AuthorisedAction.ChangeBomType, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var part = new Part
                           {
                               PartNumber = "TEST 001",
                               BomType = "C",
                               PreferredSupplier = new Supplier { SupplierId = 1, Name = "Shugs Shoes" }
                           };
            this.partRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);

            this.partSupplierRepository.FindBy(Arg.Any<Expression<Func<PartSupplier, bool>>>()).Returns((PartSupplier)null);

            this.bomTypeChange = new BomTypeChange
                                     {
                                         PartNumber = "TEST 001",
                                         OldBomType = "C",
                                         OldSupplierId = 1,
                                         NewBomType = "A",
                                         NewSupplierId = 2
                                     };


            this.action = () => this.Sut.ChangeBomType(this.bomTypeChange, new List<string>());
        }

        [Test]
        public void ShouldThrowInvalidBomTypeChangeException()
        {
            this.action.Should().Throw<InvalidBomTypeChangeException>().WithMessage("New supplier id 2 is not a supplier of part TEST 001");
        }
    }
}
