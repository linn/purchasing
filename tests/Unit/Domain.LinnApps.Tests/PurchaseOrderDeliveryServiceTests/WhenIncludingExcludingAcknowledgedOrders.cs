﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenIncludingExcludingAcknowledgedOrders : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SearchDeliveries(null, null, false);
        }

        [Test]
        public void ShouldOnlyReturnExactSupplierMatches()
        {
            Assert.IsTrue(
                this.result.All(x => !x.AdvisedDate.HasValue));
        }
    }
}

