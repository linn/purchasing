namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
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

            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
        }
    }
}
