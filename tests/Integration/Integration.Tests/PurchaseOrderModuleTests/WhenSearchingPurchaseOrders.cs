﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingPurchaseOrders : ContextBase
    {
        private string orderNumberSearch;

        private List<PurchaseOrderResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.orderNumberSearch = "600179";

            this.dataResult = new List<PurchaseOrderResource>
                                  {
                                      new PurchaseOrderResource
                                          {
                                              OrderNumber = 600179,
                                              Cancelled = string.Empty,
                                              OrderDate = 10.January(2021),
                                              Overbook = string.Empty,
                                              OverbookQty = 1,
                                              Supplier = new SupplierResource { Id = 1224 }
                                          }
                                  };

            this.MockPurchaseOrderRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>()).Returns(
                new List<PurchaseOrder>
                    {
                        new PurchaseOrder
                            {
                                OrderNumber = 600179,
                                Cancelled = string.Empty,
                                OrderDate = 10.January(2021),
                                Overbook = string.Empty,
                                OverbookQty = 1,
                                Supplier = new Supplier { SupplierId = 1224 }
                            }
                    }.AsQueryable());

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders?searchTerm={this.orderNumberSearch}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<PurchaseOrderResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            resources?.First().OrderNumber.Should().Be(600179);
        }
    }
}
