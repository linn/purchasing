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
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PCAS 100/1" }, // board
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "RES 10K/2" },  // non-board
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "PCB 200/2" },  // board
                new PartUsedOn { RootProduct = "ROOTX", PartNumber = "CAP 1UF/1" },  // non-board
                new PartUsedOn { RootProduct = "ROOTY", PartNumber = "PCSM 300/3" }  // board, other root
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
                        new BomChange { DocumentType = "CRF", BomName = "PCAS 100/1" },
                        new BomChange { DocumentType = "CRF", BomName = "RES 10K/2" }  
                    }
                },
                new ChangeRequest
                {
                    BoardCode = "888",
                    DocumentNumber = 14,
                    BomChanges = new List<BomChange>
                    {
                        new BomChange { DocumentType = "CRF", BomName = "CAP 1UF/1" }
                    }
                }
            };

            this.Repository.FindAll().Returns(changeRequests.AsQueryable());
        }

        [Test]
        public void ShouldReturnChangeRequestsForValidBoardPartsOnly()
        {
            var result = this.Sut.GetForRootProducts("ROOTX").ToList();
            
            result.Should().HaveCount(3);
            result.Should().ContainSingle(r => r.BoardCode == "100");
            result.Should().ContainSingle(r => r.BoardCode == "200");
            result.Should().ContainSingle(r => r.DocumentNumber == 13);
            
            result.Should().OnlyContain(r =>
                r.BoardCode == "100" ||
                r.BoardCode == "200" ||
                r.DocumentNumber == 13);
        }
    }
}
