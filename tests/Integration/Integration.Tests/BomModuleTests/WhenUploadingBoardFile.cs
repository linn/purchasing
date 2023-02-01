namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUploadingBoardFile : ContextBase
    {
        private string revisionCode;

        private string fileType;

        [SetUp]
        public void SetUp()
        {
            this.revisionCode = "L1R1";
            this.fileType = "TSB";
            this.CircuitBoardService.UpdateFromFile(
                    this.BoardCode,
                    this.revisionCode,
                    this.fileType,
                    Arg.Any<string>())
                .Returns(new ProcessResult(true, "ok"));
            this.Response = this.Client.Post(
                "/purchasing/purchase-orders/boms/upload-board-file",
                "\"C001\", \"CAP 401\"",
                with =>
                    {
                        with.Accept("application/json");
                    },
                "text/tab-separated-values").Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Test]
        public void ShouldReturnProcessResource()
        {
            var result = this.Response.DeserializeBody<ProcessResultResource>();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("ok");
        }
    }
}
