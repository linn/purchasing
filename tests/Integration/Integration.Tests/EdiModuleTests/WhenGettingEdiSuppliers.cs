namespace Linn.Purchasing.Integration.Tests.EdiModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingEdiSuppliers : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var suppliers = new List<EdiSupplier>
                                {
                                    new EdiSupplier { SupplierId = 1, SupplierName = "Sunak Audio" },
                                    new EdiSupplier { SupplierId = 2, SupplierName = "Penny Audio" }
                                };
            this.MockDomainService.GetEdiSuppliers().Returns(suppliers);

            this.Response = this.Client.Get(
                "/purchasing/edi/suppliers",
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
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().GetEdiSuppliers();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<List<EdiSupplierResource>>;
        }
    }
}
