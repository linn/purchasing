namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPostingBom : ContextBase
    {
        private BomTreeNode tree;

        private PostBomResource resource;

        [SetUp]
        public void SetUp()
        {
            var c1 = new BomTreeNode {Name = "COMP", Type = "C" };
            var c2 = new BomTreeNode { Name = "ASS", Type = "A" };

            this.tree = new BomTreeNode
                            { 
                                Name = "BOMBOM", 
                                Children = new List<BomTreeNode>
                                               {
                                                   c1, c2
                                               }
                            };

            this.resource = new PostBomResource
                                {
                                    CrNumber = 4567,
                                    TreeRoot = this.tree
                                };

            this.BomChangeService.CreateBomChanges(
                Arg.Any<BomTreeNode>(),
                4567,
                Arg.Any<int>()).Returns(this.resource.TreeRoot);

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/boms/tree",
                this.resource).Result;
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
