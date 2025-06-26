namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingSupplierAssEmail : ContextBase
    {
        private int orderNumber;

        [SetUp]
        public void SetUp()
        {
            this.orderNumber = 158962;
            this.MockDomainService.SendSupplierAssemblyEmail(this.orderNumber)
                .Returns(new ProcessResult(true, "email sent"));

            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/email-supplier-ass?orderNumber={this.orderNumber}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ProcessResultResource>();
            resource.Should().NotBeNull();
            resource.Success.Should().BeTrue();
            resource.Message.Should().Be("email sent");
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
    }
}
