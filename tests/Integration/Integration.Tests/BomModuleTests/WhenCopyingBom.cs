namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;
    using System.Net.Http.Json;
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCopyingBom : ContextBase
    {
        private BomFunctionResource functionResource;

        [SetUp]
        public void SetUp()
        {
            this.functionResource 
                = new BomFunctionResource
                      {
                          SrcPartNumber = "SRC", DestPartNumber = "DEST", CrfNumber = 123
                      };
            this.BomTreeService.BuildBomTree("DEST", null, false, true)
                .Returns(new BomTreeNode { Name = "DEST" });
            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/boms/copy",
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
            var result = this.Response.DeserializeBody<BomTreeNode>();
            result.Name.Should().Be("DEST");
        }
    }
}
