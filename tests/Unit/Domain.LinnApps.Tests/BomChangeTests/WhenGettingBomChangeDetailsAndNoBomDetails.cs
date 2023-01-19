namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingBomChangeDetailsAndNoBomDetails : ContextBase
    {
        private IEnumerable<BomChangeDetail> Results;

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
            this.Results = this.Sut.BomChangeDetails();
        }

        [Test]
        public void ShouldHaveTwoChangeDetails()
        {
            this.Results.Count().Should().Be(0);
        }
    }
}
