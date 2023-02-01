namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;
    using NSubstitute.ReturnsExtensions;

    using NUnit.Framework;

    public class WhenCreatingNewBom : ContextBase
    {
        private BomTreeNode result;

        private Part part;

        [SetUp]
        public void Setup()
        {
            this.part = new Part { PartNumber = "NEW BOM" };

            // since bom doesn't exist yet
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).ReturnsNull();

            this.DatabaseService.GetIdSequence("BOM_SEQ").Returns(123);
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(456);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(this.part);

            this.result = this.Sut.ProcessTreeUpdate(
                new BomTreeNode
                    {
                        Name = "NEW BOM", HasChanged = true, Children = new List<BomTreeNode>()
                    }, 
                666, 
                33087);
        }

        [Test]
        public void ShouldCreateBom()
        {
            this.BomRepository.Received().Add(Arg.Is<Bom>(
                b => b.BomId == 123 
                     && b.BomName == this.part.PartNumber
                     && b.Depth == 1
                     && b.CommonBom == "N"));
        }

        [Test]
        public void ShouldCreateBomChange()
        {
            this.BomChangeRepository.Received().Add(
                Arg.Is<BomChange>(c 
                    => c.ChangeId == 456
                       && c.BomId == 123
                       && c.BomName == this.part.PartNumber
                       && c.DocumentType == "CRF"
                       && c.DocumentNumber == 666
                       && c.PartNumber == this.part.PartNumber
                       && c.DateEntered == DateTime.Today
                       && c.EnteredById == 33087
                       && c.ChangeState == "PROPOS"
                       && c.Comments == "BOM_UT"
                       && c.PcasChange == "N"));
        }

        [Test]
        public void ShouldSetPartBomId()
        {
            this.part.BomId.Should().Be(123);
        }
    }
}
