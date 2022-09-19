namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var bom = new Bom
                                    {
                                       BomName = "BOM BOM",
                                       BomId = 1,
                                       Details = new List<BomDetail>
                                                     {
                                                         new BomDetail
                                                             {
                                                                 BomName = "COM",
                                                                 BomId = 1,
                                                                 DetailId = 1,
                                                                 PartNumber = "COM",
                                                                 Part = new Part
                                                                            {
                                                                                BomType = "C",
                                                                                PartNumber = "COM",
                                                                                Description = "COMPONENT"
                                                                            }
                                                             }
                                                     }
                                    };

            this.Repository.FindById(1).Returns(bom);

            this.Response = this.Client.Get(
                "/purchasing/boms/1",
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
        public void ShouldCallRepository()
        {
            this.Repository.Received().FindById(1);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<BomResource>();
            result.Should().NotBeNull();
            result.BomId.Should().Be(1);
            result.Details.Count().Should().Be(1);
            result.Details.First().PartNumber.Should().Be("COM");
            result.Details.First().BomType.Should().Be("C");
            result.Details.First().PartDescription.Should().Be("COMPONENT");
        }
    }
}
