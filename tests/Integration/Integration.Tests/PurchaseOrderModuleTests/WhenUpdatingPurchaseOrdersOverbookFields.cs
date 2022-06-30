﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPurchaseOrders : ContextBase
    {
        private PurchaseOrderResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderResource
                                {
                                    OrderNumber = 600179,
                                    Supplier = new SupplierResource { Id = 1111, Name = "seller" }
                                };

            this.MockPurchaseOrderRepository.FindById(600179).Returns(
                new PurchaseOrder
                    {
                        OrderNumber = 600179,
                        Supplier = new Supplier { SupplierId = 1111 }
                    });

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/purchase-orders/600179",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.MockDomainService.Received()
                .UpdateOrder(
                    Arg.Any<PurchaseOrder>(),
                    Arg.Any<PurchaseOrder>(),
                    Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            var response = this.Response;

            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
