namespace Linn.Purchasing.Integration.Tests.PrefSupReceiptsReportModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingExport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var csv = new List<List<string>>
                          {
                              new List<string> { "mary", "little", "lamb" },
                              new List<string> { "princess", "frog", "kiss" }
                          };

            this.ReportFacadeService.GetExport("abc1234567", "xyz1234567")
                .Returns(csv);
            this.Response = this.Client.Get(
                "/purchasing/reports/pref-sup-receipts/export?fromDate=abc1234567&toDate=xyz1234567",
                with => { with.Accept("application/csv"); }).Result;
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
            this.ReportFacadeService.Received().GetExport("abc1234567", "xyz1234567");
        }
    }
}
