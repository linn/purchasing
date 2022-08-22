namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorisingMultiplePurchaseOrders : ContextBase
    {
        private PurchaseOrdersProcessRequestResource requestResource;

        [SetUp]
        public void SetUp()
        {
            this.requestResource = new PurchaseOrdersProcessRequestResource { Orders = new List<int> { 234, 567 } };
            this.MockDomainService.AuthoriseMultiplePurchaseOrders(
                Arg.Is<List<int>>(a => a.Contains(234) && a.Contains(567)),
                Arg.Any<int>())
                .Returns(new ProcessResult(true, "authorised ok"));

            this.Response = this.Client.Post(
                "/purchasing/purchase-orders/authorise-multiple",
                this.requestResource,
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ProcessResultResource>();
            resource.Should().NotBeNull();
            resource.Success.Should().BeTrue();
            resource.Message.Should().Be("authorised ok");
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
        public void ShouldCommit()
        {
            this.TransactionManager.Received().Commit();
        }
    }
}
