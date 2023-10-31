namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenAutoBookAlreadyBooked : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.PurchaseOrder.AuthorisedById = 808;
            this.Sut.Part.StockControlled = "N";
            this.Sut.OurQty = 1;
            this.Sut.PurchaseDeliveries.First().QtyNetReceived = 1;

            this.result = this.Sut.CanBeAutoBooked();
        }

        [Test]
        public void ShouldBeFalse()
        {
            this.result.Should().BeFalse();
        }
    }
}
