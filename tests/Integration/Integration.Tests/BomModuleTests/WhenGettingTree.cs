﻿namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingTree : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.BomTreeService.BuildTree("root").Returns(new BomTreeNode { Name = "root", Description = "root node"});

            this.Response = this.Client.Get(
                "/purchasing/boms/tree?bomName=root",
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
        public void ShouldReturnData()
        {
            var result = this.Response.DeserializeBody<BomTreeNode>();
            result.Should().NotBeNull();
            result.Name.Should().Be("root");
            result.Description.Should().Be("root node");
        }
    }
}
