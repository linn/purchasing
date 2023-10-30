namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenAutoBookNotAuth : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.PurchaseOrder.AuthorisedById = null;
            this.Sut.Part.StockControlled = "N";
            this.Sut.OurQty = 1;

            this.result = this.Sut.CanBeAutoBooked();
        }

        [Test]
        public void ShouldBeFalse()
        {
            this.result.Should().BeFalse();
        }
    }
}
