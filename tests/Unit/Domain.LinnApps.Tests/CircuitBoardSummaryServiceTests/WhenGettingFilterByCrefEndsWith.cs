namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardSummaryServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenGettingFilterByCrefEndsWith : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Expression = this.Sut.GetFilterExpression(null, null, "*6", null);
            this.Results = this.Components.Where(this.Expression);
        }

        [Test]
        public void ShouldSelectCorrectComponents()
        {
            this.Results.Should().HaveCount(1);
            this.Results.Should().Contain(a => a.Cref == "R456");
        }
    }
}
