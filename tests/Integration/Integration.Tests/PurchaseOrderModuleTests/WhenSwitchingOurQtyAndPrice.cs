namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSwitchingOurQtyAndPrice : ContextBase
    {
        private int orderNumber;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 537634;
            this.MockDomainService.SwitchOurQtyAndPrice(this.orderNumber, Arg.Any<int>(), Arg.Any<List<string>>())
                .Returns(
                    new PurchaseOrder
                        {
                            OrderNumber = this.orderNumber, Supplier = new Supplier(), OrderDate = DateTime.Now
                        });

            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/{this.orderNumber}/switch-our-qty-price",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.OrderNumber.Should().Be(this.orderNumber);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallService()
        {
            this.MockDomainService.Received().SwitchOurQtyAndPrice(
                this.orderNumber,
                Arg.Any<int>(),
                Arg.Any<List<string>>());
        }
    }
}
