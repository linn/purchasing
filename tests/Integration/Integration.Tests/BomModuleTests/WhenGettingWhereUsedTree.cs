namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingWhereUsedTree : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.BomTreeService.BuildWhereUsedTree("PART").Returns(new BomTreeNode { Name = "part", Description = "root node" });

            this.Response = this.Client.Get(
                "/purchasing/boms/tree?bomName=part&requirementOnly=true&showChanges=false&treeType=whereUsed",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallService()
        {
            this.BomTreeService.Received().BuildWhereUsedTree("PART", null, true, false);
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
        public void ShouldReturnData()
        {
            var result = this.Response.DeserializeBody<BomTreeNode>();
            result.Should().NotBeNull();
            result.Name.Should().Be("part");
            result.Description.Should().Be("root node");
        }
    }
}