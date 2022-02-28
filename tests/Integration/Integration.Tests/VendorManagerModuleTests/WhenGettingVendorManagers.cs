namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
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

    public class WhenGettingVendorManagers : ContextBase
    {
        private List<VendorManagerResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.dataResult = new List<VendorManagerResource>
                                  {
                                      new VendorManagerResource
                                          {
                                              UserNumber = 33333,
                                              VmId = "P",
                                              Name = "Mario"
                                          }
                                  };

            this.VendorManagerFacadeService.GetAll()
                .Returns(new SuccessResult<IEnumerable<VendorManagerResource>>(this.dataResult));

            this.Response = this.Client.Get(
                $"/purchasing/vendor-managers",
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
            var resources = this.Response.DeserializeBody<IEnumerable<VendorManagerResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().Name.Should().Be("Mario");
        }
    }
}
