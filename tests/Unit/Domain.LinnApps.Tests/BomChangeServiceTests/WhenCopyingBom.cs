namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCopyingBom : ContextBase
    {
        private Bom destBom;

        private BomChange change;

        [SetUp]
        public void Setup()
        {
            this.destBom = new Bom
                               {
                                   BomId = 123,
                                   BomName = "DEST"
                               };
            this.change = new BomChange
                              {
                                  ChangeId = 456, 
                                  DocumentNumber = 666, 
                                  DocumentType = "CRF", 
                                  ChangeState = "PROPOS",
                                  BomId = 123
                              };
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(this.destBom);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "BOM", BomType = "A", DecrementRule = "YES" });
            this.BomChangeRepository.FilterBy(Arg.Any<Expression<Func<BomChange, bool>>>()).Returns(
                new List<BomChange> { this.change }.AsQueryable());
            this.Sut.CopyBom("SRC", this.destBom.BomName, 33087, 666, "O");
        }

        [Test]
        public void ShouldCallStoredProcedure()
        {
            this.BomPack.Received().CopyBom("SRC", this.destBom.BomId, this.change.ChangeId, "PROPOS", "O");
        }
    }
}
