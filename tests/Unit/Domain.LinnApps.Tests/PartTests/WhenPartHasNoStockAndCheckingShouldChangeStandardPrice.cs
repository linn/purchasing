namespace Linn.Purchasing.Domain.LinnApps.Tests.PartTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using NUnit.Framework;

    public class WhenPartHasNoStockAndCheckingShouldChangeStandardPrice : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new Part
            {
                PartNumber = "Test001",
                BomType = "C",
                LinnProduced = "N",
                PreferredSupplier = new Supplier { SupplierId = 1234 },
                BaseUnitPrice = 12
            };
        }

        [Test]
        public void ShouldChangeStandardPrice()
        {
            this.Sut.ShouldChangeStandardPrice(0).Should().BeTrue();
        }
    }
}
