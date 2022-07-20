namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAutomaticPurchaseOrder : ContextBase
    {
        private AutomaticPurchaseOrderResource automaticPurchaseOrder;

        private int id;

        [SetUp]
        public void SetUp()
        {
            this.id = 123;
            this.automaticPurchaseOrder = new AutomaticPurchaseOrderResource
                                              {
                                                  Id = this.id, Details = new List<AutomaticPurchaseOrderDetailResource>()
                                              };

            this.FacadeService.GetById(this.id, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<AutomaticPurchaseOrderResource>(this.automaticPurchaseOrder));

            this.Response = this.Client.Get(
                $"/purchasing/automatic-purchase-orders/{this.id}",
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
            var resource = this.Response.DeserializeBody<AutomaticPurchaseOrderResource>();
            resource.Should().NotBeNull();

            resource.Id.Should().Be(this.id);
        }
    }
}
