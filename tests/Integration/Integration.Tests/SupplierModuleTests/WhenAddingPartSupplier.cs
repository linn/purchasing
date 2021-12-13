namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingPartSupplier : ContextBase
    {
        private PartSupplierResource resource;

        [SetUp]
        public void SetUp()
        {

            this.resource = new PartSupplierResource
            {
                PartNumber = "PART",
                SupplierId = 100
            };

            this.PartSupplierFacadeService.Add( Arg.Any<PartSupplierResource>())
                .ReturnsForAnyArgs(new CreatedResult<PartSupplierResource>(
                                       new PartSupplierResource
                                           {
                                               PartNumber = "PART",
                                               SupplierId = 100
                                           }));

            this.Response = this.Client.Post(
                $"/purchasing/part-suppliers/",
                this.resource,
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.PartSupplierFacadeService
                .Add(Arg.Any<PartSupplierResource>(), Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            var response = this.Response;

            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PartSupplierResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
