namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingSupplier : ContextBase
    {
        private SupplierResource resource;

        [SetUp]
        public void SetUp()
        {

            this.resource = new SupplierResource
            {
            };

            this.MockDomainService.CreateSupplier(Arg.Any<Supplier>(), Arg.Any<IEnumerable<string>>())
                .Returns(new Supplier());
            this.Response = this.Client.Post(
                $"/purchasing/suppliers",
                this.resource,
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
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
            var resultResource = this.Response.DeserializeBody<SupplierResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
