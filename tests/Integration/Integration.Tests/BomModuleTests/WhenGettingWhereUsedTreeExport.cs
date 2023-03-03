namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingWhereUsedTreeExport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.BomTreeService.FlattenBomTree("root").Returns(
                new List<BomTreeNode> { new BomTreeNode { Name = "part", Description = "root node" } });

            this.Response = this.Client.Get(
                "/purchasing/boms/tree/flat?bomName=part&requirementOnly=true&showChanges=false&treeType=whereUsed",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallService()
        {
            this.BomTreeService.Received().FlattenWhereUsedTree("PART", null, true, false);
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnCsvContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("text/csv; charset=utf-8");
        }
    }
}
