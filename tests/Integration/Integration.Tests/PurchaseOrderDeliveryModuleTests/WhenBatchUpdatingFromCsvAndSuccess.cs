namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingFromCsvAndSuccess : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.BatchUploadDeliveries(
                Arg.Any<IEnumerable<PurchaseOrderDeliveryUpdate>>(),
                Arg.Any<IEnumerable<string>>()).Returns(new BatchUpdateProcessResult
                                                             {
                                                                 Success = true,
                                                                 Message = "Success!"
                                                             });
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/deliveries",
                $"PO1,1,28-mar-1995,100,$0.01,NEW REASON,",
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
        public void ShouldPassCorrectDataToDomainService()
        {
            this.MockDomainService.Received().BatchUploadDeliveries(
                Arg.Is<IEnumerable<PurchaseOrderDeliveryUpdate>>(
                    l => l.First().Key.OrderNumber.Equals(1)
                    && l.First().Key.OrderLine.Equals(1)
                    && l.First().NewDateAdvised.Equals(28.March(1995))
                    && l.First().Qty.Equals(100)
                    && l.First().UnitPrice.Equals(0.01m)
                    && l.First().NewReason.Equals("NEW REASON")),
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
