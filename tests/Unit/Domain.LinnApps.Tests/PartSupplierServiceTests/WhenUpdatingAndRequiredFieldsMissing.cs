namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndRequiredFieldsMissing : ContextBase
    {
        private Action action;

        private PartSupplier current;

        private PartSupplier updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PartSupplier
                               {
                                   PartNumber = "PART",
                                   SupplierId = 1,
                                   SupplierDesignation = string.Empty
                               };
            this.updated = new PartSupplier
                               {
                                   PartNumber = "PART",
                                   SupplierId = 1,
                                   SupplierDesignation = "We updated this to this."
                               };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = () => this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.current.SupplierDesignation.Should().Be(string.Empty);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>();
        }

        [Test]
        public void ShouldListMissingFieldsInMessage()
        {
            this.action.Should().Throw<PartSupplierException>()
                .WithMessage(
                    "The inputs for the following fields are empty/invalid: Minimum Order Qty, Created By, Order Increment, Lead Time Weeks, Rohs Category, Order Method, Currency Unit Price, Minimum Delivery Quantity, Damages Percent, Currency, ");
        }
    }
}
