namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSearchingByOrderNumber : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SearchDeliveries(null, "123455", true);
        }

        [Test]
        public void ShouldOnlyReturnDeliveriesForOrdersThatStartWithTheSearchTerm()
        {
            Assert.IsTrue(
                this.result.All(x => x.OrderNumber.ToString().Equals("123455")));
        }

        [Test]
        public void ShouldExcludeCancelledOrders()
        {
            this.result.Count().Should().Be(1);
            Assert.IsTrue(
                this.result.All(x => x.Cancelled != "N"));
        }
    }
}
