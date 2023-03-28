namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUploadingSmtFile : ContextBase
    {
        private string revisionCode;

        private string fileType;

        private int pcasChangeId;

        [SetUp]
        public void SetUp()
        {
            this.revisionCode = "L1R1";
            this.fileType = "SMT";
            this.pcasChangeId = 678;
            this.PcasChangeRepository.FindBy(Arg.Any<Expression<Func<PcasChange, bool>>>()).Returns(
                new PcasChange
                    {
                        ChangeId = this.pcasChangeId, ChangeRequest = new ChangeRequest { BoardCode = this.BoardCode }
                    });
            this.CircuitBoardService.UpdateFromFile(
                    this.BoardCode,
                    this.revisionCode,
                    this.fileType,
                    Arg.Any<string>(),
                    null,
                    false)
                .Returns(new ProcessResult(true, "ok"));
            this.Response = this.Client.Post(
                $"/purchasing/purchase-orders/boms/upload-smt-file?boardCode={this.BoardCode}&revisionCode={this.revisionCode}&changeRequestId={this.pcasChangeId}",
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
