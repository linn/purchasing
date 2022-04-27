namespace Linn.Purchasing.Integration.Tests.ForecastingModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApplyingPercentageChange : ContextBase
    {
        private ApplyForecastingPercentageChangeResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new ApplyForecastingPercentageChangeResource
                                {
                                    Change = 12, 
                                    StartPeriod = 1, 
                                    EndPeriod = 2
                                };
            this.MockDomainService.ApplyPercentageChange(
                    this.resource.Change, this.resource.StartPeriod, this.resource.EndPeriod, Arg.Any<IEnumerable<string>>())
                .Returns(new ProcessResult(true, "Success"));

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/forecasting/apply-percentage-change",
                this.resource).Result;
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
            this.MockDomainService.Received().ApplyPercentageChange(
                this.resource.Change,
                this.resource.StartPeriod,
                this.resource.EndPeriod,
                Arg.Any<List<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<ProcessResultResource>();
            resultResource.Success.Should().BeTrue();
            resultResource.Message.Should().Be("Success");
        }
    }
}
