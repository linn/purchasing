namespace Linn.Purchasing.Integration.Tests.ForecastingModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Response = this.Client.Get(
               "/purchasing/forecasting/apply-percentage-change/report",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("Forecast % Change Report");
        }
    }
}
