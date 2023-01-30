namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingBomChangeDetailsForAReplace : ContextBase
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
                               AddedBomDetails =
                                   new List<BomDetail>
                                       {
                                           new BomDetail
                                               {
                                                   DetailId = 1,
                                                   PartNumber = "CONN 1",
                                                   Qty = 1,
                                                   GenerateRequirement = "Y",
                                                   AddChangeId = 1,
                                                   AddReplaceSeq = 1
                                               }
                                       },
                               DeletedBomDetails = new List<BomDetail>
                                                       {
                                                           new BomDetail
                                                               {
                                                                   DetailId = 1,
                                                                   PartNumber = "LBL 12",
                                                                   Qty = 2,
                                                                   GenerateRequirement = "N",
                                                                   DeleteChangeId = 1,
                                                                   DeleteReplaceSeq = 1
                                                               }
                                                       }
                           };
            this.results = this.Sut.BomChangeDetails();
        }

        [Test]
        public void ShouldHaveOneChangeDetails()
        {
            this.results.Count().Should().Be(1);
        }

        [Test]
        public void ShouldHaveRightSingleChangeDetails()
        {
            var detail = this.results.First();
            detail.Should().NotBeNull();
            detail.AddPartNumber.Should().Be("CONN 1");
            detail.AddQty.Should().Be(1);
            detail.AddGenerateRequirement.Should().Be("Y");
            detail.DeletePartNumber.Should().Be("LBL 12");
            detail.DeleteQty.Should().Be(2);
            detail.DeleteGenerateRequirement.Should().Be("N");
        }
    }
}
