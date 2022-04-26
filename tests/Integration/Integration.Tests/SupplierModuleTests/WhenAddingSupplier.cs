namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

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
                Name = "SUPPLIER"
            };

            this.MockDomainService.CreateSupplier(Arg.Any<Supplier>(), Arg.Any<IEnumerable<string>>())
                .Returns(new Supplier
                             {
                                 Name = "SUPPLIER",
                                 SupplierId = 123
                             });

            this.MockDatabaseService.GetNextVal("SUPPLIER_SEQ").Returns(123);

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/suppliers",
                this.resource).Result;
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
        public void ShouldQuerySequence()
        {
            this.MockDatabaseService.Received().GetNextVal("SUPPLIER_SEQ");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<SupplierResource>();
            resultResource.Should().NotBeNull();
            resultResource.Name.Should().Be("SUPPLIER");
            resultResource.Id.Should().Be(123);
        }
    }
}
