namespace Linn.Purchasing.Integration.Tests.SupplierGroupModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAllSupplierGroups : ContextBase
    {
        private List<SupplierGroupResource> result;

        [SetUp]
        public void SetUp()
        {
            this.result = new List<SupplierGroupResource>
                              {
                                  new SupplierGroupResource { Id = 1, Name = "sg1" },
                                  new SupplierGroupResource { Id = 2, Name = "sg2" }
                              };

            this.SupplierGroupFacadeService.GetAll(Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<IEnumerable<SupplierGroupResource>>(this.result));

            this.Response = this.Client.Get(
                $"/purchasing/supplier-groups",
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
        public void ShouldCallFacadeService()
        {
            this.SupplierGroupFacadeService.Received().GetAll(Arg.Any<IEnumerable<string>>());
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
            var resources = this.Response.DeserializeBody<IEnumerable<SupplierGroupResource>>().ToArray();
            resources.Should().HaveCount(2);
            resources.First(a => a.Id == 1).Name.Should().Be("sg1");
            resources.First(a => a.Id == 2).Name.Should().Be("sg2");
        }
    }
}
