namespace Linn.Purchasing.Integration.Tests.PrefSupReceiptsReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingExport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.ReportFacadeService.GetReport("abc1234567", "xyz1234567")
                .Returns(new SuccessResult<ReportReturnResource>(new ReportReturnResource()));
            this.Response = this.Client.Get(
                "/purchasing/reports/pref-sup-receipts/export?fromDate=abc1234567&toDate=xyz1234567",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldReturnCsvContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("text/csv");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallFacade()
        {
            this.ReportFacadeService.Received().GetReport("abc1234567", "xyz1234567");
        }
    }
}
