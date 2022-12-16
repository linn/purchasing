namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenMakingLivePcasChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new PcasChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
            var employee = new Employee { Id = 1, FullName = "Stevie Croft" };
            this.Sut.MakeLive(employee);
        }

        [Test]
        public void ShouldBeLive()
        {
            this.Sut.ChangeState.Should().Be("LIVE");
            this.Sut.DateApplied.Should().NotBeNull();
            this.Sut.AppliedBy.Should().NotBeNull();
            this.Sut.AppliedById.Should().Be(1);
        }
    }
}
