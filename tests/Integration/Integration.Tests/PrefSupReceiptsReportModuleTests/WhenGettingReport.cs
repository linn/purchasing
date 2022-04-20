namespace Linn.Purchasing.Integration.Tests.PrefSupReceiptsReportModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var reportReturnResource = new ReportReturnResource();
            var reportResult = new ReportResultResource
                                   {
                                       title = new DisplayResource("hello"),
                                   };

            reportReturnResource.ReportResults.Add(reportResult);
            this.ReportFacadeService.GetReport("abc", "xyz")
                .Returns(new SuccessResult<ReportReturnResource>(reportReturnResource));
            this.Response = this.Client.Get(
                "/purchasing/reports/pref-sup-receipts/report?fromDate=abc&toDate=xyz",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallFacade()
        {
            this.ReportFacadeService.Received().GetReport("abc", "xyz");
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("hello");
        }
    }
}
