namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenAutoBookQtyIsWrong : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.PurchaseOrder.AuthorisedById = 808;
            this.Sut.Part.StockControlled = "N";
            this.Sut.OurQty = 2;

            this.result = this.Sut.CanBeAutoBooked();
        }

        [Test]
        public void ShouldBeFalse()
        {
            this.result.Should().BeFalse();
        }
    }
}
