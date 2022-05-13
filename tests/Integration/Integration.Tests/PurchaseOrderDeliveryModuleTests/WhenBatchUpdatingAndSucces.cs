namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingAndSuccess : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.BatchUpdateDeliveries(
                Arg.Any<IEnumerable<PurchaseOrderDeliveryUpdate>>(),
                Arg.Any<IEnumerable<string>>()).Returns(new BatchUpdateProcessResult
                                                            {
                                                                Success = true,
                                                                Message = "Success!"
                                                            });
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                $"PO1,28/03/1995,NEW REASON",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
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
            var resultResource = this.Response.DeserializeBody<BatchUpdateProcessResultResource>();
            resultResource.Success.Should().Be(true);
            resultResource.Message.Should().Be("Success!");
        }
    }
}
