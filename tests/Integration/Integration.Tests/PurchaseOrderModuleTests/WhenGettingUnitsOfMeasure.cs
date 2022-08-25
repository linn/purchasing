namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
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

    public class WhenGettingUnitsOfMeasure : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.UnitsOfMeasureService.GetAll().Returns(
                new SuccessResult<IEnumerable<UnitOfMeasureResource>>(
                    new[]
                        {
                            new UnitOfMeasureResource { Unit = "ONES" }, new UnitOfMeasureResource { Unit = "TWOS" }
                        }));

            this.Response = this.Client.Get(
                "/purchasing/purchase-orders/units-of-measure",
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
            var resources = this.Response.DeserializeBody<IEnumerable<UnitOfMeasureResource>>().ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources.First().Unit.Should().Be("ONES");
            resources.ElementAt(1).Unit.Should().Be("TWOS");
        }
    }
}
