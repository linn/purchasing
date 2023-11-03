namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenPartNotPopulated : ContextBase
    {
        private bool result;

        [SetUp]
        public void SetUp()
        {
            this.Sut.PurchaseOrder.AuthorisedById = 808;
            this.Sut.Part = null;
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
