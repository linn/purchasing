namespace Linn.Purchasing.Integration.Tests.BomReportsModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenGettingBoardComponentSummaryBadRequest : ContextBase
    {
        private string boardCode;

        private string revisionCode;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "1";
            this.revisionCode = "L1R1";

            this.DomainService
                .GetBoardComponentSummaryReport(this.boardCode, this.revisionCode)
                .Throws(new ItemNotFoundException("not found"));

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/board-component-summary/report?boardCode={this.boardCode}&revisionCode={this.revisionCode}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetBoardComponentSummaryReport(
                this.boardCode,
                this.revisionCode);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<string>();
            resource.Should().Be("not found");
        }
    }
}
