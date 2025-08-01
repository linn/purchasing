﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingFromCsvAndErrors : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.UploadDeliveries(
                Arg.Any<IEnumerable<PurchaseOrderDeliveryUpdate>>(),
                Arg.Any<IEnumerable<string>>()).Returns(
                    new UploadPurchaseOrderDeliveriesResult
                    {
                        Success = false,
                        Message = "Something went wrong!",
                        Errors = new List<Error>
                                     {
                                         new Error("Id", "Message")
                                     },
                        Updated = new List<PurchaseOrderDelivery>()
                    });
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                "PO1,28/03/1995,100,0.01,NEW REASON,",
                with =>
                    {
                        with.Accept("application/json");
                    },
                "text/csv").Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Test]
        public void ShouldCommitChanges()
        {
            this.MockTransactionManager.Received()
                .Commit();
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
            var resultResource = this.Response.DeserializeBody<BatchUpdateProcessResultResourceWithLinks>();
            resultResource.Success.Should().Be(false);
            resultResource.Message.Should().Be("Something went wrong!");
            resultResource.Errors.First().Descriptor.Should().Be("Id");
            resultResource.Errors.First().Message.Should().Be("Message");
        }
    }
}
