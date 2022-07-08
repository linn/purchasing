namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAutomaticPurchaseOrder : ContextBase
    {
        private AutomaticPurchaseOrderResource resource;

        private int id;

        [SetUp]
        public void SetUp()
        {
            this.id = 123;

            this.resource = new AutomaticPurchaseOrderResource
                                {
                                    Id = this.id, Details = new List<AutomaticPurchaseOrderDetailResource>()
                                };

            this.FacadeService.Update(
                    this.id,
                    Arg.Is<AutomaticPurchaseOrderResource>(a => a.Id == this.id),
                    Arg.Any<List<string>>(),
                    Arg.Any<int>())
                .Returns(
                    new SuccessResult<AutomaticPurchaseOrderResource>(
                        new AutomaticPurchaseOrderResource
                            {
                                Id = this.id,
                                Details = new List<AutomaticPurchaseOrderDetailResource>(),
                                Links = new[] { new LinkResource("self", $"/purchasing/automatic-purchase-orders/{this.id}") }
                            }));

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/automatic-purchase-orders/{this.id}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.FacadeService.Received()
                .Update(
                    this.id,
                    Arg.Is<AutomaticPurchaseOrderResource>(a => a.Id == this.id),
                    Arg.Any<List<string>>(),
                    Arg.Any<int>());
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
            var resultResource = this.Response.DeserializeBody<AutomaticPurchaseOrderResource>();
            resultResource.Should().NotBeNull();

            resultResource.Id.Should().Be(this.id);
        }
    }
}
