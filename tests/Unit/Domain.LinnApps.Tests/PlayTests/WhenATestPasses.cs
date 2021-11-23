namespace Linn.Purchasing.Domain.LinnApps.Tests.PlayTests
{
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenATestPasses
    {
        private Thing thing;

        [SetUp]
        public void SetUp()
        {
            this.thing = new Thing { Id = 1, Name = "new" };
        }

        [Test]
        public void ShouldBeAThing()
        {
            this.thing.Id.Should().Be(1);
            this.thing.Name.Should().Be("new");
        }
    }
}
