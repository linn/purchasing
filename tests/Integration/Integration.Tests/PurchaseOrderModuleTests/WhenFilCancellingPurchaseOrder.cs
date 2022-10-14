﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFilCancellingPurchaseOrder : ContextBase
    {
        private PurchaseOrderResource from;

        private PurchaseOrderResource to;

        private PurchaseOrder order;

        private int filCancelledBy;

        [SetUp]
        public void SetUp()
        {
            this.filCancelledBy = 123;
            this.order = new PurchaseOrder
            {
                OrderNumber = 600179,
                Supplier = new Supplier(),
                Cancelled = "N",
                DocumentType = new DocumentType { Description = "order", Name = "RO" },
                OverbookQty = null,
                Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           OrderPosting =
                                                               new PurchaseOrderPosting
                                                                   {
                                                                       NominalAccount = new NominalAccount(),
                                                                   }
                                                       }
                                               },
                Currency = new Currency(),
                OrderMethod = new OrderMethod(),
                DeliveryAddress = new LinnDeliveryAddress(),
                RequestedBy = new Employee(),
                EnteredBy = new Employee(),
                AuthorisedBy = new Employee(),
                OrderAddress = new Address()
            };
            this.from = new PurchaseOrderResource
            {
                OrderNumber = 600179
            };

            this.to = new PurchaseOrderResource
            {
                OrderNumber = 600179,
                Details = new List<PurchaseOrderDetailResource>
                                              {
                                                  new PurchaseOrderDetailResource
                                                      {
                                                          Line = 1,
                                                          FilCancelled = "Y",
                                                          FilCancelledBy = this.filCancelledBy,
                                                          ReasonFilCancelled = "some reason"
                                                      }
                                              }
            };
            var resource = new PatchRequestResource<PurchaseOrderResource>
            {
                From = this.from,
                To = this.to,
            };
            this.MockPurchaseOrderRepository
                .FindById(this.from.OrderNumber).Returns(
                    new PurchaseOrder
                    {
                        OrderNumber = this.from.OrderNumber
                    });

            this.MockNominalAccountRepository
                .FindById(Arg.Any<int>()).Returns(new NominalAccount());

            this.MockDomainService.FilCancelLine(
                this.from.OrderNumber,
                1,
                this.filCancelledBy,
                this.to.Details.First().ReasonFilCancelled,
                Arg.Any<IEnumerable<string>>()).Returns(this.order);

            this.Response = this.Client.PatchAsync(
                $"/purchasing/purchase-orders/{this.from.OrderNumber}",
                JsonContent.Create(resource)).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received(1).Commit();
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().FilCancelLine(
                this.from.OrderNumber,
                this.to.Details.First().Line,
                this.filCancelledBy,
                this.to.Details.First().ReasonFilCancelled,
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderDeliveryResource>();
            resultResource.Should().NotBeNull();
            resultResource.OrderNumber.Should().Be(this.from.OrderNumber);
        }
    }
}
