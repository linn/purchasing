namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsPlanningServiceTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRunningMrpButAlreadyRunning : ContextBase
    {
        private ProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.MasterRepository.GetRecord()
                .Returns(new MrMaster { JobRef = "abc", RunLogIdCurrentlyInProgress = 345 });
            this.result = this.Sut.RunMrp();
        }

        [Test]
        public void ShouldNotGetId()
        {
            this.MrpLoadPack.DidNotReceive().GetNextRunLogId();
        }

        [Test]
        public void ShouldNotRunMrp()
        {
            this.MrpLoadPack.DidNotReceive().ScheduleMrp(Arg.Any<int>());
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("MRP is already in progress");
        }
    }
}
