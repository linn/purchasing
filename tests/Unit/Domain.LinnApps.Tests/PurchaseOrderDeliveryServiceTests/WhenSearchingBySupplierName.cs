namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSearchingBySupplierName : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SearchDeliveries("supplier 4", null, true);
        }

        [Test]
        public void ShouldReturnAllRecordsWithSupplierNameContainingSearchTerm()
        {
            Assert.That(
                this.result.All(x => x.PurchaseOrderDetail.PurchaseOrder.Supplier.Name.Contains("SUPPLIER 4")));
        }
    }
}
