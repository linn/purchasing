namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NUnit.Framework;

    public class WhenAddingCircuitBoard : ContextBase
    {
        private CircuitBoardResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new CircuitBoardResource
                                {
                                    BoardCode = "123",
                                    Description = "Desc",
                                    ClusterBoard = "Y",
                                    CoreBoard = "Y",
                                    IdBoard = "Y",
                                    SplitBom = "Y"
                                };

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/boms/boards",
                this.resource).Result;
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
            var resultResource = this.Response.DeserializeBody<CircuitBoardResource>();
            resultResource.Should().NotBeNull();
            resultResource.BoardCode.Should().Be(this.resource.BoardCode);
            resultResource.Description.Should().Be(this.resource.Description);
        }
    }
}
