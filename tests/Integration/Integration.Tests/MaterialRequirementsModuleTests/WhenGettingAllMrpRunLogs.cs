namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAllMrpRunLogs : ContextBase
    {
        private List<MrpRunLogResource> result;

        [SetUp]
        public void SetUp()
        {
            this.result = new List<MrpRunLogResource>
                              {
                                  new MrpRunLogResource { MrRunLogId = 1, JobRef = "A", LoadMessage = "abc" },
                                  new MrpRunLogResource { MrRunLogId = 2, JobRef = "B", LoadMessage = "def" }
                              };

            this.MrpRunLogFacadeService.GetAll(Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<IEnumerable<MrpRunLogResource>>(this.result));

            this.Response = this.Client.Get(
                $"/purchasing/material-requirements/run-logs",
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
            this.MrpRunLogFacadeService.Received().GetAll(Arg.Any<IEnumerable<string>>());
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
            var resources = this.Response.DeserializeBody<IEnumerable<MrpRunLogResource>>()?.ToArray();
            resources.Should().HaveCount(2);
            resources?.First(a => a.MrRunLogId == 1).JobRef.Should().Be("A");
            resources?.First(a => a.MrRunLogId == 1).LoadMessage.Should().Be("abc");
            resources?.First(a => a.MrRunLogId == 2).JobRef.Should().Be("B");
            resources?.First(a => a.MrRunLogId == 2).LoadMessage.Should().Be("def");
        }
    }
}
