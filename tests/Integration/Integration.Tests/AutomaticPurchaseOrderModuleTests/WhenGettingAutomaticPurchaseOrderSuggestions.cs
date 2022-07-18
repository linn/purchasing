namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAutomaticPurchaseOrderSuggestions : ContextBase
    {
        private IEnumerable<AutomaticPurchaseOrderSuggestionResource> results;

        private int? planner;

        private int? supplierId;

        [SetUp]
        public void SetUp()
        {
            this.planner = 22;
            this.supplierId = 33333;
            this.results = new List<AutomaticPurchaseOrderSuggestionResource>
                               {
                                   new AutomaticPurchaseOrderSuggestionResource { PartNumber = "P1" },
                                   new AutomaticPurchaseOrderSuggestionResource { PartNumber = "P2" }
                               };
            this.SuggestionFacadeService.FilterBy(
                    Arg.Is<PlannerSupplierRequestResource>(
                        a => a.Planner == this.planner && a.SupplierId == this.supplierId))
                .Returns(new SuccessResult<IEnumerable<AutomaticPurchaseOrderSuggestionResource>>(this.results));

            this.Response = this.Client.Get(
                $"/purchasing/automatic-purchase-order-suggestions?supplierId={this.supplierId}&planner={this.planner}",
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
        public void ShouldCallFacadeService()
        {
            this.SuggestionFacadeService.Received(1).FilterBy(
                Arg.Is<PlannerSupplierRequestResource>(
                    a => a.Planner == this.planner && a.SupplierId == this.supplierId));
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
            var resources = this.Response.DeserializeBody<IEnumerable<AutomaticPurchaseOrderSuggestionResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.PartNumber == "P1");
            resources.Should().Contain(a => a.PartNumber == "P2");
        }
    }
}
