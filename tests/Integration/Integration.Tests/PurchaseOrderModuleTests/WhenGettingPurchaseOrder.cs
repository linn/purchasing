namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPurchaseOrder : ContextBase
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
                               Cancelled = string.Empty,
                               DocumentTypeName = string.Empty,
                               OrderDate = 10.January(2021),
                               Overbook = string.Empty,
                               OverbookQty = 1,
                               SupplierId = 1224,
                               Supplier = new Supplier { SupplierId = 1224 },
                               LedgerEntries = new List<PurchaseLedger>
                                                   {
                                                       new PurchaseLedger
                                                           {
                                                               Pltref = 123,
                                                               DocumentUploaded = "Y"
                                                           },
                                                       new PurchaseLedger
                                                           {
                                                               Pltref = 456,
                                                               DocumentUploaded = "N"
                                                           }
                                                   }
            };

            this.MockPurchaseOrderRepository.FindById(this.orderNumber).Returns(
               this.order);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/{this.orderNumber}",
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.OrderNumber.Should().Be(this.order.OrderNumber);
            resource.OverbookQty.Should().Be(this.order.OverbookQty);
            resource.Supplier.Id.Should().Be(this.order.SupplierId);
        }

        [Test]
        public void ShouldBuildLedgerEntryLinks()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.LedgerEntries.Single(x => x.Tref == 123).Links.Any(x => x.Rel == "invoice-pdf").Should().BeTrue();
            resource.LedgerEntries.Single(x => x.Tref == 456).Links.Any(x => x.Rel == "invoice-pdf").Should().BeFalse();
        }
    }
}
