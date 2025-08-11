namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrp : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MaterialRequirementsPlanningFacadeService.RunMrp(Arg.Any<List<string>>())
                .Returns(new SuccessResult<ProcessResultResourceWithLinks>(new ProcessResultResourceWithLinks(true, "Running")));

            this.Response = this.Client.Post(
                $"/purchasing/material-requirements/run-mrp",
                string.Empty,
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
        public void ShouldCallService()
        {
            this.MaterialRequirementsPlanningFacadeService.Received().RunMrp(Arg.Any<List<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<ProcessResultResourceWithLinks>();
            resultResource.Success.Should().BeTrue();
            resultResource.Message.Should().Be("Running");
        }
    }
}
