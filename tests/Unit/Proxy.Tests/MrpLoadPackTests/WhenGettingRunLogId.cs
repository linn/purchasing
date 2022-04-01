namespace Linn.Purchasing.Proxy.Tests.MrpLoadPackTests
{
    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingRunLogId : ContextBase
    {
        private int runLogId = 123456;

        private string sequenceName = "MR_RUNLOG_SEQ";

        private int result;

        [SetUp]
        public void SetUp()
        {
            this.DatabaseService.GetIdSequence(this.sequenceName).Returns(this.runLogId);
            this.result = this.Sut.GetNextRunLogId();
        }

        [Test]
        public void ShouldCallDatabaseService()
        {
            this.DatabaseService.Received().GetIdSequence(this.sequenceName);
        }


        [Test]
        public void ShouldReturnId()
        {
            this.result.Should().Be(this.runLogId);
        }
    }
}
