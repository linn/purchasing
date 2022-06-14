namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSearchingBySpecificOrderLine : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SearchDeliveries(null, "223453", true, true, 3);
        }

        [Test]
        public void ShouldOnlyReturnDeliveriesForThatOrderLine()
        {
            Assert.IsTrue(
                this.result.All(
                    x => x.OrderNumber.ToString().StartsWith("223453")
                    && x.OrderLine == 3));
        }
    }
}
