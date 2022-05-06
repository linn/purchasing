namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsPlanningServiceTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrp : ContextBase
    {
        private ProcessResult result;

        private int runLogId;

        [SetUp]
        public void SetUp()
        {
            this.runLogId = 1234;
            this.MrpLoadPack.GetNextRunLogId().Returns(this.runLogId);
            this.MasterRepository.GetRecord().Returns(new MrMaster { JobRef = "abc" });
            this.MrpLoadPack.ScheduleMrp(this.runLogId).Returns(new ProcessResult(true, "ok"));
            this.result = this.Sut.RunMrp();
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("ok");
            this.result.ProcessHref.Should().Be($"/purchasing/material-requirements/run-logs/{this.runLogId}");
        }
    }
}
