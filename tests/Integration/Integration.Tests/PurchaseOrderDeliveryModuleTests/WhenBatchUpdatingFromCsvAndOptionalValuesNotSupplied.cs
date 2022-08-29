namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;
    
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingFromCsvAndOptionalValuesNotSupplied : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.UploadDeliveries(
                Arg.Any<IEnumerable<PurchaseOrderDeliveryUpdate>>(),
                Arg.Any<IEnumerable<string>>()).Returns(new UploadPurchaseOrderDeliveriesResult
                {
                    Success = true,
                    Message = "Success!",
                    Updated = new List<PurchaseOrderDelivery>()
                });
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                $"PO1,,100,$0.01,,",
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
        public void ShouldPassDefaults()
        {
            this.MockDomainService.Received().UploadDeliveries(
                Arg.Is<IEnumerable<PurchaseOrderDeliveryUpdate>>(
                    l => l.First().Key.OrderNumber.Equals(1)
                    && l.First().NewDateAdvised.Equals(1.January(2025)) // date defaults to this future date
                    && l.First().Qty.Equals(100)
                    && l.First().UnitPrice.Equals(0.01m)
                    && l.First().NewReason.Equals("ADVISED")), // reason defaults to 'ADVISED'
                Arg.Any<IEnumerable<string>>());
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

