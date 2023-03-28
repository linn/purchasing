namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardSummaryServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingFilterByCrefMatches : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Expression = this.Sut.GetFilterExpression(null, null, "C*1", null);
            this.Results = this.Components.Where(this.Expression);
        }

        [Test]
        public void ShouldSelectCorrectComponents()
        {
            this.Results.Should().HaveCount(2);
            this.Results.Should().Contain(a => a.Cref == "C001");
            this.Results.Should().Contain(a => a.Cref == "C011");
        }
    }
}
