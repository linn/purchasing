namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingFromJsonAndSuccess : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdateResource> resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new List<PurchaseOrderDeliveryUpdateResource>
                                {
                                    new PurchaseOrderDeliveryUpdateResource
                                        {
                                            OrderNumber = 1,
                                            OrderLine = 1,
                                            DeliverySequence = 1,
                                            DateRequested = DateTime.Now
                                        }
                                };
            this.MockDomainService.UpdateDeliveries(
                Arg.Any<IEnumerable<PurchaseOrderDeliveryUpdate>>(),
                Arg.Any<IEnumerable<string>>()).Returns(new BatchUpdateProcessResult { Success = true });
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                this.resource,
                with => { with.Accept("application/json"); },
                "application/json").Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCommitChanges()
        {
            this.MockTransactionManager.Received().Commit();
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
            var resultResource = this.Response.DeserializeBody<BatchUpdateProcessResultResource>();
            resultResource.Success.Should().Be(true);
        }
    }
}
