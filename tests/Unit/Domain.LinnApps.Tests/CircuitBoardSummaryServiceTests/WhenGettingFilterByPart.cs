namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardSummaryServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingFilterByPart : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Expression = this.Sut.GetFilterExpression(null, null, null, "P1");
            this.Results = this.Components.Where(this.Expression);
        }

        [Test]
        public void ShouldSelectCorrectComponents()
        {
            this.Results.Should().HaveCount(2);
            this.Results.All(a => a.PartNumber == "P1").Should().BeTrue();
        }
    }
}
