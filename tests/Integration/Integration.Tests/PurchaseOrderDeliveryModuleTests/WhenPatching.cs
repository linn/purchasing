namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPatching : ContextBase
    {
        private int orderNumber;

        private int orderLine;

        private int deliverySequence;

        private PatchRequestResource<PurchaseOrderDeliveryResource> resource;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 123456;
            this.orderLine = 1;
            this.deliverySequence = 1;
            this.resource = new PatchRequestResource<PurchaseOrderDeliveryResource>();
            this.resource.To = new PurchaseOrderDeliveryResource
                                {
                                    DateAdvised = DateTime.Today.ToString("o"),
                                    RescheduleReason = "REASON",
                                    SupplierConfirmationComment = "Comment",
                                    AvailableAtSupplier = "Y"
                                };
            this.resource.From = new PurchaseOrderDeliveryResource();

            this.MockRepository
                .FindById(
                    Arg.Is<PurchaseOrderDeliveryKey>(
                        k => 
                            k.OrderNumber == this.orderNumber 
                            && k.OrderLine == this.orderLine
                            && k.DeliverySequence == this.deliverySequence))
                .Returns(new PurchaseOrderDelivery
                             {
                                 OrderNumber = this.orderNumber,
                                 OrderLine = this.orderLine,
                                 DeliverySeq = this.deliverySequence,
                                 PurchaseOrderDetail = new PurchaseOrderDetail { PartNumber = "PART" }
                             });

            // todo - could add PatchAsJsonAsync() extension to HttpClient for convention's sake
            this.Response = this.Client.PatchAsync( 
                $"/purchasing/purchase-orders/deliveries/{this.orderNumber}/{this.orderLine}/{this.deliverySequence}",
                JsonContent.Create(this.resource)).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCommit()
        {
            this.MockTransactionManager.Received(1).Commit();
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
            var resultResource = this.Response.DeserializeBody<PurchaseOrderDeliveryResource>();
            resultResource.Should().NotBeNull();
            resultResource.OrderNumber.Should().Be(this.orderNumber);
        }
    }
}
