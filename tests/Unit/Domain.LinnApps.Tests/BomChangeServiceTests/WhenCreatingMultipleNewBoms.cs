using Linn.Purchasing.Domain.LinnApps.Boms.Models;
using Linn.Purchasing.Domain.LinnApps.Boms;
using Linn.Purchasing.Domain.LinnApps.Parts;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using FluentAssertions;

    using NSubstitute.ReturnsExtensions;

    public class WhenCreatingMultipleNewBoms : ContextBase
    {
        private Part rootPart;

        private Part subAssembly;

        private Part component;

        [SetUp]
        public void Setup()
        {
            this.rootPart = new Part { PartNumber = "ROOTPART", BomType = "A" };

            this.subAssembly = new Part { PartNumber = "SUB1", DecrementRule = "YES", BomType = "A" };

            this.component = new Part { PartNumber = "C1", DecrementRule = "YES", BomType = "C" };

            // since boms don't exist yet
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).ReturnsNull();

            this.DatabaseService.GetIdSequence("BOM_SEQ").Returns(123, 124);
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(456, 457);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(this.rootPart, this.subAssembly, this.subAssembly, this.component, this.component);
            
            this.Sut.ProcessTreeUpdate(
                new BomTreeNode
                {
                    Name = "ROOTPART",
                    AssemblyHasChanges = true,
                    Children = new List<BomTreeNode>
                                   {
                                       new BomTreeNode
                                           {
                                               Name = "SUB1",
                                               AssemblyHasChanges = true,
                                               ParentName = "ROOTPART",
                                               Children = new List<BomTreeNode>
                                                              {
                                                                  new BomTreeNode { Name = "C1", ParentName = "SUB1" }
                                                              }
                                           }
                                   }
                },
                666,
                33087);
        }

        [Test]
        public void ShouldCreateBoms()
        {
            this.BomRepository.Received(2).Add(Arg.Any<Bom>());
            this.BomRepository.Received().Add(Arg.Is<Bom>(
                b => b.BomId == 123
                     && b.BomName == this.rootPart.PartNumber
                     && b.Depth == 1
                     && b.CommonBom == "N"));
            this.BomRepository.Received().Add(Arg.Is<Bom>(
                b => b.BomId == 124
                     && b.BomName == this.subAssembly.PartNumber
                     && b.Depth == 1
                     && b.CommonBom == "N"));
        }

        [Test]
        public void ShouldCreateBomChanges()
        {
            this.BomChangeRepository.Received(2).Add(Arg.Any<BomChange>());

            this.BomChangeRepository.Received().Add(
                Arg.Is<BomChange>(c
                    => c.ChangeId == 456
                       && c.BomId == 123
                       && c.BomName == this.rootPart.PartNumber
                       && c.DocumentType == "CRF"
                       && c.DocumentNumber == 666
                       && c.PartNumber == this.rootPart.PartNumber
                       && c.DateEntered == DateTime.Today
                       && c.EnteredById == 33087
                       && c.ChangeState == "PROPOS"
                       && c.Comments == "BOM_UT"
                       && c.PcasChange == "N"));
            this.BomChangeRepository.Received().Add(
                Arg.Is<BomChange>(c
                    => c.ChangeId == 457
                       && c.BomId == 124
                       && c.BomName == this.subAssembly.PartNumber
                       && c.DocumentType == "CRF"
                       && c.DocumentNumber == 666
                       && c.PartNumber == this.subAssembly.PartNumber
                       && c.DateEntered == DateTime.Today
                       && c.EnteredById == 33087
                       && c.ChangeState == "PROPOS"
                       && c.Comments == "BOM_UT"
                       && c.PcasChange == "N"));
        }

        [Test]
        public void ShouldSetPartBomIds()
        {
            this.rootPart.BomId.Should().Be(123);
            this.subAssembly.BomId.Should().Be(124);
        }

        [Test]
        public void ShouldAddBomDetails()
        {
            this.BomDetailRepository.Received(2).Add(Arg.Any<BomDetail>());
        }
    }
}
