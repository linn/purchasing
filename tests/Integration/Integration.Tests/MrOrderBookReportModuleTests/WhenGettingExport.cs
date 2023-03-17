namespace Linn.Purchasing.Integration.Tests.MrOrderBookReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingExport : ContextBase
    {
        private int supplierId;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 1;
            
            this.MockDomainService.GetOrderBookExport(
                    this.supplierId)
                .Returns(new ResultsModel());

            this.Response = this.Client.Get(
                $"/purchasing/reports/mr-order-book?supplierId={this.supplierId}",
                with => { with.Accept("text/csv"); })?.Result;
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetOrderBookExport(this.supplierId);
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
    }
}
