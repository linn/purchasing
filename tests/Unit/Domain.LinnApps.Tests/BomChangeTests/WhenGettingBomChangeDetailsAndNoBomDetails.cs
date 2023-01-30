namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingBomChangeDetailsAndNoBomDetails : ContextBase
    {
        private IEnumerable<BomChangeDetail> results;

        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               ChangeId = 1,
                               DocumentNumber = 1,
                               BomName = "TOM",
                               ChangeState = "PROPOS",
                               AddedBomDetails = new List<BomDetail>(),
                               DeletedBomDetails = new List<BomDetail>()
            };
            this.results = this.Sut.BomChangeDetails();
        }

        [Test]
        public void ShouldHaveTwoChangeDetails()
        {
            this.results.Count().Should().Be(0);
        }
    }
}
