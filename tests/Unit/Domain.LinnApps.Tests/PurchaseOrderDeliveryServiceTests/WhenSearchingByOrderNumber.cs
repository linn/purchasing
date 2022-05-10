namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSearchingByOrderNumber : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SearchDeliveries(null, "12345", true);
        }

        [Test]
        public void ShouldOnlyReturnDeliveriesForOrdersThatStartWithTheSearchTerm()
        {
            Assert.IsTrue(
                this.result.All(x => x.OrderNumber.ToString().StartsWith("12345")));
        }
    }
}
