namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPreferredSupplierChange : ContextBase
    {
        private PreferredSupplierChangeResource resource;

        [SetUp]
        public void SetUp()
        {

            this.resource = new PreferredSupplierChangeResource
                                {
                                    PartNumber = "PART",
                                };

            this.PreferredSupplierChangeService.Add(
                    Arg.Any<PreferredSupplierChangeResource>(), 
                    Arg.Any<IEnumerable<string>>())
                .ReturnsForAnyArgs(new CreatedResult<PreferredSupplierChangeResource>(
                    new PreferredSupplierChangeResource
                        {
                            PartNumber = "PART",
                        }));

            this.Response = this.Client.Post(
                $"/purchasing/preferred-supplier-changes",
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
        public void ShouldCallAdd()
        {
            this.PreferredSupplierChangeService
                .Add(Arg.Any<PreferredSupplierChangeResource>(), Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PreferredSupplierChangeResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
