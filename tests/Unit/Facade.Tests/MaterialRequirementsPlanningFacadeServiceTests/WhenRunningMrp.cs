namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsPlanningFacadeServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrp : ContextBase
    {
        private IEnumerable<string> privileges;

        private IResult<ProcessResultResource> result;

        [SetUp]
        public void SetUp()
        {
            this.privileges = new List<string>();

            this.MaterialRequirementsPlanningService.RunMrp()
                .Returns(new ProcessResult(true, "ok") { ProcessHref = "/mrp/123" });

            this.result = this.Sut.RunMrp(this.privileges);
        }

        [Test]
        public void ShouldCallService()
        {
            this.MaterialRequirementsPlanningService.Received().RunMrp();
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<ProcessResultResource>>();
            var dataResult = ((SuccessResult<ProcessResultResource>)this.result).Data;
            dataResult.Success.Should().BeTrue();
            dataResult.Message.Should().Be("ok");
            dataResult.Links.First().Href.Should().Be("/mrp/123");
            dataResult.Links.First().Rel.Should().Be("status");
        }
    }
}
