namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingMrpRunLogById : ContextBase
    {
        private int id;

        private MrpRunLogResource mrpRunLog;

        [SetUp]
        public void SetUp()
        {
            this.id = 1;
            this.mrpRunLog = new MrpRunLogResource { MrRunLogId = 1, JobRef = "A" };

            this.MrpRunLogFacadeService.GetById(1, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<MrpRunLogResource>(this.mrpRunLog));

            this.Response = this.Client.Get(
                $"/purchasing/material-requirements/run-logs/{this.id}",
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
        public void ShouldCallFacadeService()
        {
            this.MrpRunLogFacadeService.Received().GetById(1, Arg.Any<IEnumerable<string>>());
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
            var resource = this.Response.DeserializeBody<MrpRunLogResource>();
            resource.MrRunLogId.Should().Be(this.mrpRunLog.MrRunLogId);
            resource.JobRef.Should().Be(this.mrpRunLog.JobRef);
        }
    }
}
