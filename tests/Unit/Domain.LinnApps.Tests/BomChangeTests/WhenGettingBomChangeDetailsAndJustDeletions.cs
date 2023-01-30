namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenGettingBomChangeDetailsAndJustDeletions : ContextBase
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
                DeletedBomDetails = new List<BomDetail>
                                        {
                                            new BomDetail
                                                {
                                                    DetailId = 1,
                                                    PartNumber = "CONN 1",
                                                    Qty = 1,
                                                    GenerateRequirement = "Y",
                                                    DeleteChangeId = 1
                                                },
                                            new BomDetail
                                                {
                                                    DetailId = 1,
                                                    PartNumber = "LBL 12",
                                                    Qty = 2,
                                                    GenerateRequirement = "Y",
                                                    DeleteChangeId = 1
                                                }
                                        }
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
            var conn = this.results.Single(d => d.DeletePartNumber == "CONN 1");
            conn.Should().NotBeNull();
            conn.DeleteQty.Should().Be(1);
            conn.AddPartNumber.Should().BeNull();

            var lbl = this.results.Single(d => d.DeletePartNumber == "LBL 12");
            lbl.Should().NotBeNull();
            lbl.DeleteQty.Should().Be(2);
            lbl.AddPartNumber.Should().BeNull();
        }
    }
}
