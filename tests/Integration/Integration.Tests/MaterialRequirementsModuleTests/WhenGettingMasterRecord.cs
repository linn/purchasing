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

    public class WhenGettingMasterRecord : ContextBase
    {
        private IResult<MrMasterResource> result;
        private MrMasterResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new MrMasterResource
            {
                JobRef = "A",
                RunDate = "1.February(2025)"
            };

            this.result = new SuccessResult<MrMasterResource>(this.resource);

            this.MasterRecordFacadeService.Get(Arg.Any<IEnumerable<string>>())
                .Returns(this.result);

            this.Response = this.Client.Get(
                "/purchasing/material-requirements/last-run",
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
            this.MasterRecordFacadeService.Received().Get(Arg.Any<IEnumerable<string>>());
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
            var resultResource = this.Response.DeserializeBody<MrMasterResource>();
            resultResource.JobRef.Should().Be(this.resource.JobRef);
            resultResource.RunDate.Should().Be(this.resource.RunDate);
        }
    }
}
