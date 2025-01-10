namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingVendorManagerById : ContextBase
    {
        private string id;

        private VendorManager vendorManager;

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManager 
                                     { 
                                         Id = this.id,
                                         UserNumber = 10, 
                                         Employee = new Employee 
                                                        { 
                                                            FullName = "vm1", 
                                                            Id = 10
                                                        }
                                     };

            this.VendorManagerRepository.FindById(this.id).Returns(this.vendorManager);

            this.Response = this.Client.Get(
                $"/purchasing/vendor-managers/{this.id}",
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
            var resource = this.Response.DeserializeBody<VendorManagerResource>();
            resource.VmId.Should().Be(this.id);
        }
    }
}
