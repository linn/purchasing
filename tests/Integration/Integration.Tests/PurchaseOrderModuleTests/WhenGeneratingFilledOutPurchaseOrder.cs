namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGeneratingFilledOutPurchaseOrder : ContextBase
    {
        private PurchaseOrder order;

        [SetUp]
        public void SetUp()
        {
            this.order = new PurchaseOrder
                             {
                                 Cancelled = string.Empty,
                                 DocumentTypeName = "PO",
                                 OrderDate = 10.January(2021),
                                 SupplierId = 1111,
                                 Supplier = new Supplier { SupplierId = 1111, Name = "seller" },
                                 Currency = new Currency { Code = "EUR" },
                                 CurrencyCode = "EUR",
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           BaseNetTotal = 100m,
                                                           NetTotalCurrency = 120m,
                                                           OurQty = 12m,
                                                           OrderQty = 12m,
                                                           PartNumber = "macbookz",
                                                           OurUnitPriceCurrency = 120m,
                                                           OrderUnitPriceCurrency = 120m,
                                                           BaseOurUnitPrice = 100m,
                                                           BaseOrderUnitPrice = 100m,
                                                           DetailTotalCurrency = 120m,
                                                           BaseDetailTotal = 100m,
                                                       }
                                               },
                                 DeliveryAddress = new LinnDeliveryAddress
                                                       {
                                                           AddressId = 5678,
                                                           Description = "Linn",
                                                           IsMainDeliveryAddress = "Y",
                                                           FullAddress = new FullAddress { AddressString = "Linn HQ, waterfut" }
                                                       }
            };

            this.MockDomainService.FillOutUnsavedOrder(Arg.Any<PurchaseOrder>(), Arg.Any<int>()).Returns(this.order);

            var resource = new PurchaseOrderResource
            {
                Supplier = new SupplierResource { Id = 1111, Name = "seller" },
                Details =
                                       new List<PurchaseOrderDetailResource>
                                           {
                                                new PurchaseOrderDetailResource
                                                    {
                                                        Line = 1,
                                                        BaseNetTotal = 100m,
                                                        NetTotalCurrency = 120m,
                                                        OurQty = 12m,
                                                        OrderQty = 12m,
                                                        PartNumber = "macbookz",
                                                        OurUnitPriceCurrency = 120m,
                                                        OrderUnitPriceCurrency = 120m,
                                                        BaseOurUnitPrice = 100m,
                                                        BaseOrderUnitPrice = 100m,
                                                        DetailTotalCurrency = 120m,
                                                        BaseDetailTotal = 100m,
                                                    }
                                           },
            };

            this.Response = this.Client.Post(
                                $"/purchasing/purchase-orders/generate-order-from-supplier-id",
                                resource,
                                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();

            resource.Supplier.Id.Should().Be(this.order.SupplierId);
            resource.DeliveryAddress.AddressId.Should().Be(5678);
            resource.Currency.Code.Should().Be("EUR");
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallDomain()
        {
            this.MockDomainService.Received().FillOutUnsavedOrder(Arg.Any<PurchaseOrder>(), Arg.Any<int>());
        }
    }
}
