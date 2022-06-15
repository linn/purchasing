namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingMrOrders : ContextBase
    {
        private string jobRef;

        private MrRequestResource request;

        private MrPurchaseOrdersResource orders;

        [SetUp]
        public void SetUp()
        {
            this.jobRef = "A123";
            this.request =
                new MrRequestResource { JobRef = this.jobRef, PartNumbers = new List<string> { "abc", "def" } };
            this.orders = new MrPurchaseOrdersResource();
            this.MaterialRequirementsReportFacadeService
                .GetMaterialRequirementOrders(Arg.Any<MrRequestResource>(), Arg.Any<List<string>>())
                .Returns(new SuccessResult<MrPurchaseOrdersResource>(this.orders));
            this.Response = this.Client.PostAsJsonAsync("/purchasing/material-requirements/orders", this.request).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.MaterialRequirementsReportFacadeService.Received().GetMaterialRequirementOrders(
                Arg.Is<MrRequestResource>(
                    a => a.JobRef == this.jobRef && a.PartNumbers.Contains("abc") && a.PartNumbers.Contains("def")),
                Arg.Any<IEnumerable<string>>());
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
            var resource = this.Response.DeserializeBody<MrPurchaseOrdersResource>();
        }
    }
}
