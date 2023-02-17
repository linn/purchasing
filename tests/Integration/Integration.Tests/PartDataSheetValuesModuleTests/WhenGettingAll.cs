namespace Linn.Purchasing.Integration.Tests.PartDataSheetValuesModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAll : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PartDataSheetValuesRepository.FindAll().Returns(
                new List<PartDataSheetValues>
                    {
                        new PartDataSheetValues
                            {
                                Field = "F"
                            }, 
                        new PartDataSheetValues
                            {
                                Field = "G"
                            }
                    }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/part-data-sheet-values/",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
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
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PartDataSheetValuesResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.Field == "F");
            resources.Should().Contain(a => a.Field == "G");
        }
    }
}
