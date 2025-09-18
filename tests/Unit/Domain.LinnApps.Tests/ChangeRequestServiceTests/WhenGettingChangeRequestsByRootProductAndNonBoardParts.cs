namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NSubstitute;
    using NUnit.Framework;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class WhenGettingChangeRequestsByRootProductAndNonBoardParts : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var partUsedOns = new List<PartUsedOn>
            {
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PCAS 100/1" }, // board, matching root prod
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PART/2" },  // non-board, not on any bom change
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PCB 200/2" },  // board, matching root prod
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PART/1" },  // non-board, but on a bom change
                new PartUsedOn { RootProduct = "ROOTY", PartNumber = "PCSM 300/3" }  // board, other root product
            };

            this.PartUsedOnRepository.FindAll().Returns(partUsedOns.AsQueryable());

            var changeRequests = new List<ChangeRequest>
            {
                new ChangeRequest { BoardCode = "100", DocumentNumber = 10 },
                new ChangeRequest { BoardCode = "200", DocumentNumber = 11 },
                new ChangeRequest { BoardCode = "300", DocumentNumber = 12 },
                new ChangeRequest
                {
                    BoardCode = "999",
                    DocumentNumber = 13,
                    BomChanges = new List<BomChange>
                    {
                        new BomChange { DocumentType = "CRF", BomName = "PCAS 100/1"  }
                    }
                },
                new ChangeRequest
                {
                    BoardCode = "888",
                    DocumentNumber = 14,
                    BomChanges = new List<BomChange>
                    {
                        new BomChange { DocumentType = "CRF", BomName = "PART/1" }
                    }
                }
            };

            this.Repository.FindAll().Returns(changeRequests.AsQueryable());
        }

        [Test]
        public void ShouldReturnChangeRequestsForValidBoardPartsOnly()
        {
            var result = this.Sut.GetForRootProducts("ROOTX").ToList();
            
            result.Should().HaveCount(4);
            result.Should().ContainSingle(r => r.DocumentNumber == 10); // since this crfs board code, 100 has root product ROOTX
            result.Should().ContainSingle(r => r.DocumentNumber == 11); // since this crfs board code, 200 also has root product ROOTX
            result.Should().ContainSingle(r => r.DocumentNumber == 14); // since BomName PART/1 is on a BomChange on this crf
            result.Should().ContainSingle(r => r.DocumentNumber == 13);  // since BomName PCAS 100/1 iis on a BomChange on this crf
        }
    }
}
