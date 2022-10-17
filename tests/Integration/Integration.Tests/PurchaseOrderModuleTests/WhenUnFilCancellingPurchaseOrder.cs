namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
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

    public class WhenUnFilCancellingPurchaseOrder : ContextBase
    {
        private PurchaseOrderResource from;

        private PurchaseOrderResource to;

        private PurchaseOrder order;

        [SetUp]
        public void SetUp()
        {
            this.order = new PurchaseOrder
            {
                OrderNumber = 600179,
                Supplier = new Supplier(),
                Cancelled = "Y",
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
                                          Line = 2,
                                          FilCancelled = "N"
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

            this.MockDomainService.UnFilCancelLine(
                this.from.OrderNumber,
                2,
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
            this.MockDomainService.Received().UnFilCancelLine(
                this.from.OrderNumber,
                2,
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
