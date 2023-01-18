namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDeletingAllFromBom : ContextBase
    {
        private BomFunctionResource functionResource;

        [SetUp]
        public void SetUp()
        {
            this.functionResource = new BomFunctionResource { DestPartNumber = "DEST", CrfNumber = 123 };

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/boms/delete",
                this.functionResource).Result;
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

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<ProcessResultResource>();
            result.Success.Should().BeTrue();
        }
    }
}
