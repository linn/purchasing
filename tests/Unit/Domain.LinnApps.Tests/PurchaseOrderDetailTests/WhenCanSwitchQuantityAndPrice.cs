namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCanSwitchQuantityAndPrice : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Part.StockControlled = "N";

            this.result = this.Sut.CanSwitchOurQtyAndOurPrice();
        }

        [Test]
        public void ShouldBeTrue()
        {
            this.result.Should().BeTrue();
        }
    }
}
