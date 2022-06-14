namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingByOrderNumberWithWildcard : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.Repository.SearchByOrderWithWildcard("12345%").Returns(new List<PurchaseOrderDelivery>
                                                                            {
                                                                                new PurchaseOrderDelivery
                                                                                    {
                                                                                        OrderNumber = 123456
                                                                                    }
                                                                            });
            this.result = this.Sut.SearchDeliveries(null, "12345*", true);
        }

        [Test]
        public void ShouldCallRepoWithWildcard()
        {
            this.Repository.Received().SearchByOrderWithWildcard("12345%");
        }

        [Test]
        public void ShouldOnlyReturnResult()
        {
            Assert.IsTrue(
                this.result.First().OrderNumber.Equals(123456));
        }
    }
}
