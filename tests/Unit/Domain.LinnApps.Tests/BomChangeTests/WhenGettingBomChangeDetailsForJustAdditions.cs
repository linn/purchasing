namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingBomChangeDetailsForJustAdditions : ContextBase
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
                               AddedBomDetails = new List<BomDetail>
                                                     {
                                                         new BomDetail
                                                             {
                                                                 DetailId = 1,
                                                                 PartNumber = "CONN 1",
                                                                 Qty = 1,
                                                                 GenerateRequirement = "Y",
                                                                 AddChangeId = 1
                                                             },
                                                         new BomDetail
                                                             {
                                                                 DetailId = 1,
                                                                 PartNumber = "LBL 12",
                                                                 Qty = 2,
                                                                 GenerateRequirement = "Y",
                                                                 AddChangeId = 1
                                                             }
                                                     },
                               DeletedBomDetails = new List<BomDetail>()
                           };
            this.results = this.Sut.BomChangeDetails();
        }

        [Test]
        public void ShouldHaveTwoChangeDetails()
        {
            this.results.Count().Should().Be(2);
        }

        [Test]
        public void ShouldHaveRightChangeDetails()
        {
            var conn = this.results.Single(d => d.AddPartNumber == "CONN 1");
            conn.Should().NotBeNull();
            conn.AddQty.Should().Be(1);
            conn.DeletePartNumber.Should().BeNull();

            var lbl = this.results.Single(d => d.AddPartNumber == "LBL 12");
            lbl.Should().NotBeNull();
            lbl.AddQty.Should().Be(2);
            lbl.DeletePartNumber.Should().BeNull();
        }
    }
}
