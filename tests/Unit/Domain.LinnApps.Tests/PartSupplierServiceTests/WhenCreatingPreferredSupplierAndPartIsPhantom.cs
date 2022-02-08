namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPreferredSupplierAndPartIsPhantom : ContextBase
    {
        private Action action;

        private PreferredSupplierChange candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                 };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { BomType = "P" });
            this.action = () => this.Sut.CreatePreferredSupplierChange(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>().WithMessage("You cannot set a preferred supplier for phantoms");
        }
    }
}
