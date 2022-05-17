namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsPlanningServiceTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrpFails : ContextBase
    {
        private ProcessResult result;

        private int runLogId;

        [SetUp]
        public void SetUp()
        {
            this.runLogId = 1234;
            this.MrpLoadPack.GetNextRunLogId().Returns(this.runLogId);
            this.MasterRepository.GetRecord().Returns(new MrMaster { JobRef = "abc" });

            this.MrpLoadPack.ScheduleMrp(this.runLogId).Returns(new ProcessResult(false, "not ok"));
            this.result = this.Sut.RunMrp();
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("not ok");
            this.result.ProcessHref.Should().BeNull();
        }
    }
}
