namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    public class WhenGettingBomChangeDetailsAndJustDeletions : ContextBase
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
            this.Results = this.Sut.BomChangeDetails();
        }

        [Test]
        public void ShouldHaveTwoChangeDetails()
        {
            this.Results.Count().Should().Be(2);
        }

        [Test]
        public void ShouldHaveRightChangeDetails()
        {
            var conn = this.Results.SingleOrDefault(d => d.DeletePartNumber == "CONN 1");
            conn.Should().NotBeNull();
            conn.DeleteQty.Should().Be(1);
            conn.AddPartNumber.Should().BeNull();

            var lbl = this.Results.SingleOrDefault(d => d.DeletePartNumber == "LBL 12");
            lbl.Should().NotBeNull();
            lbl.DeleteQty.Should().Be(2);
            lbl.AddPartNumber.Should().BeNull();
        }
    }
}
