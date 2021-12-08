namespace Linn.Purchasing.Integration.Tests.PartSupplierModuleTests
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

    public class WhenGettingById : ContextBase
    {
        private PartSupplierKey key;

        private PartSupplierResource partSupplierResource;

        [SetUp]
        public void SetUp()
        {
            this.key = new PartSupplierKey
                           {
                               PartNumber = "A PART",
                               SupplierId = 1
            };
            this.partSupplierResource = new PartSupplierResource
                                            {
                                                PartNumber = "A PART", 
                                                SupplierId = 1, 
                                                SupplierName = "SUPPLIER"
                                            };

            this.FacadeService.GetById(
                    Arg.Is<PartSupplierKey>(
                        x => x.PartNumber == this.key.PartNumber && this.key.SupplierId == 1), 
                    Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<PartSupplierResource>(this.partSupplierResource));

            this.PartFacadeService.GetPartNumberFromId(100).Returns("A PART");

            this.Response = this.Client.Get(
                $"/purchasing/part-supplier?partId={100}&supplierId={1}",
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
            var resource = this.Response.DeserializeBody<PartSupplierResource>();
            resource.Should().NotBeNull();

            resource.PartNumber.Should().Be(this.key.PartNumber);
        }
    }
}
