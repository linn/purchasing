namespace Linn.Purchasing.Integration.Tests.EdiModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingEdiOrders : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var orders = new List<EdiOrder>
                             {
                                 new EdiOrder { Id = 1, OrderNumber = 1 }, new EdiOrder { Id = 1, OrderNumber = 2 }
                             };
            this.MockDomainService.GetEdiOrders(41193)
                .Returns(orders);

            this.Response = this.Client.Get(
                "/purchasing/edi/orders?supplierId=41193",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().GetEdiOrders(41193);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<List<EdiOrderResource>>;
        }
    }
}
