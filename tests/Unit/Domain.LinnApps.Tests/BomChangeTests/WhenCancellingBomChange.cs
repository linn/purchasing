namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancellingBomChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS"
                           };
            var employee = new Employee() {Id = 1, FullName = "Piers Morgan"};
            this.Sut.Cancel(employee);
        }

        [Test]
        public void ShouldBeCancelled()
        {
            this.Sut.ChangeState.Should().Be("CANCEL");
            this.Sut.DateCancelled.Should().NotBeNull();
            this.Sut.CancelledBy.Should().NotBeNull();
            this.Sut.CancelledById.Should().Be(1);
        }
    }
}
