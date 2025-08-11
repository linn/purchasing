namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCanNotSwitchQuantityAndPrice : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.Part.StockControlled = "Y";

            this.result = this.Sut.CanSwitchOurQtyAndOurPrice();
        }

        [Test]
        public void ShouldBeFalse()
        {
            this.result.Should().BeFalse();
        }
    }
}
