namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingSupplierAssEmail : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockDomainService.SendSupplierAssemblyEmail(
                Arg.Any<PurchaseOrder>(),
                Arg.Any<int>()).Returns(new ProcessResult(true, "email sent"));

            this.Response = this.Client.Post(
                "/purchasing/purchase-orders/email-supplier-ass?orderNumber=158962",
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
