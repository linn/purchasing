namespace Linn.Purchasing.Domain.LinnApps.Tests.PartTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using NUnit.Framework;

    public class WhenPartIsNewAndCheckingShouldChangeStandardPrice : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new Part
            {
                PartNumber = "Test001",
                BomType = "C",
                LinnProduced = "N",
                PreferredSupplier = null,
                BaseUnitPrice = null
            };
        }

        [Test]
        public void ShouldChangeStandardPrice()
        {
            this.Sut.ShouldChangeStandardPrice(0).Should().BeTrue();
        }
    }
}
