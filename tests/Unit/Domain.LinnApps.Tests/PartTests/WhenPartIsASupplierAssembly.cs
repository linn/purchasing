namespace Linn.Purchasing.Domain.LinnApps.Tests.PartTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NUnit.Framework;

    public class WhenPartIsASupplierAssembly : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new Part
                           {
                               PartNumber = "Test001",
                               BomType = "A",
                               LinnProduced = "Y",
                               PreferredSupplier = new Supplier {SupplierId = 1234}
                           };
        }

        [Test]
        public void ShouldBeSupplierAssembly()
        {
            this.Sut.SupplierAssembly().Should().BeTrue();
        }
    }
}
