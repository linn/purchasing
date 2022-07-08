namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingAutomaticPurchaseOrder : ContextBase
    {
        private AutomaticPurchaseOrderResource signingLimitResource;

        [SetUp]
        public void SetUp()
        {
            this.signingLimitResource = new AutomaticPurchaseOrderResource
                                            {
                                                Id = 4
                                            };

            this.FacadeService.Add(Arg.Any<AutomaticPurchaseOrderResource>(), Arg.Any<List<string>>(), Arg.Any<int>())
                .Returns(new CreatedResult<AutomaticPurchaseOrderResource>(
                    new AutomaticPurchaseOrderResource
                        {
                            Id = 4
                        }));

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/automatic-purchase-orders",
                this.signingLimitResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
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
            var resources = this.Response.DeserializeBody<AutomaticPurchaseOrderResource>();
            resources.Should().NotBeNull();

            resources.Id.Should().Be(4);
        }
    }
}
