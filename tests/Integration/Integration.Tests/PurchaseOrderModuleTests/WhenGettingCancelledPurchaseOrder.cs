namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingCancelledPurchaseOrder : ContextBase
    {
        private PurchaseOrder order;
        private int orderNumber;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 600179;
            this.order = new PurchaseOrder
            {
                OrderNumber = 600179,
                Cancelled = "Y",
                DocumentTypeName = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 1,
                SupplierId = 1224,
                Supplier = new Supplier { SupplierId = 1224 }
            };

            this.MockPurchaseOrderRepository.FindById(this.orderNumber).Returns(
               this.order);

            this.MockAuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/{this.orderNumber}",
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldNotBuildEditLink()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.Links.Any(l => l.Rel == "edit").Should().BeFalse();
        }
    }
}
