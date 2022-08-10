namespace Linn.Purchasing.Integration.Tests.LedgerPeriodModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingLedgerPeriod : ContextBase
    {
        private LedgerPeriodResource result;

        private int id;

        [SetUp]
        public void SetUp()
        {
            this.id = 123;
            this.result = new LedgerPeriodResource { PeriodNumber = this.id };

            this.LedgerPeriodFacadeService.GetById(this.id)
                .Returns(new SuccessResult<LedgerPeriodResource>(this.result));

            this.Response = this.Client.Get(
                $"/purchasing/ledger-periods/{this.id}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
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
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<LedgerPeriodResource>();
            resources.PeriodNumber.Should().Be(this.id);
        }
    }
}
