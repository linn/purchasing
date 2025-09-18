namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    
    using NSubstitute;
    using NUnit.Framework;
    
    using Linn.Purchasing.Domain.LinnApps.Boms;
    
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class WhenGettingChangeRequestsForMatchingRootProduct : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var partUsedOns = new List<PartUsedOn>
            {
                new PartUsedOn { RootProduct = "ROOT1", PartNumber = "PCAS 123/1" },
                new PartUsedOn { RootProduct = "ROOT1", PartNumber = "PCAS 456/2" },
                new PartUsedOn { RootProduct = "ROOT2", PartNumber = "PCAS 789/3" },
                new PartUsedOn { RootProduct = "ROOT3", PartNumber = "PCAS 999/9" } // unrelated
            };

            this.PartUsedOnRepository.FindAll().Returns(partUsedOns.AsQueryable());

            var changeRequests = new List<ChangeRequest>
            {
                new ChangeRequest { BoardCode = "123", DocumentNumber = 1 },
                new ChangeRequest { BoardCode = "456", DocumentNumber = 2 },
                new ChangeRequest { BoardCode = "789", DocumentNumber = 3 },
                new ChangeRequest
                {
                    BoardCode = "999",
                    DocumentNumber = 4,
                    BomChanges = new List<BomChange>
                    {
                        new BomChange { DocumentType = "CRF", BomName = "PCAS 123/1" }
                    }
                }
            };

            this.Repository.FindAll().Returns(changeRequests.AsQueryable());
        }

        [Test]
        public void ShouldReturnChangeRequestsForMatchingRootProduct()
        {
            var result = this.Sut.GetForRootProducts("ROOT1").ToList();
            
            // 2 ChangeRequests match ROOT1 part numbers directly; a 3rd matches via BomChange with a matching BomName
            result.Should().HaveCount(3);
            result.Should().ContainSingle(r => r.BoardCode == "123");
            result.Should().ContainSingle(r => r.BoardCode == "456");
            result.Should().ContainSingle(r => r.DocumentNumber == 4);
        }
    }
}
