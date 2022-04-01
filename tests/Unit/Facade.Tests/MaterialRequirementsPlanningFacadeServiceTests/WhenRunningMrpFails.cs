namespace Linn.Purchasing.Facade.Tests.MaterialRequirementsPlanningFacadeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrpFails : ContextBase
    {
        private IEnumerable<string> privileges;

        private IResult<ProcessResultResource> result;

        [SetUp]
        public void SetUp()
        {
            this.privileges = new List<string>();

            this.MaterialRequirementsPlanningService.RunMrp().Returns(new ProcessResult(false, "some bad news"));

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
            this.result.Should().BeOfType<BadRequestResult<ProcessResultResource>>();
            var dataResult = (BadRequestResult<ProcessResultResource>)this.result;
            dataResult.Message.Should().Be("some bad news");
        }
    }
}
