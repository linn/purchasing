namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingBomDetailWhenPartAlreadyOnChange : ContextBase
    {
        private Bom bom;

        private ChangeRequest request;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.bom = new Bom { BomId = 1, Part = new Part { PartNumber = "ASS 007" } };
            this.BomRepository.FindById(1).Returns(this.bom);

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(12);

            this.request = new ChangeRequest
                               {
                                   DocumentNumber = 1,
                                   ChangeState = "PROPOS",
                                   ChangeRequestType = "REPLACE",
                                   OldPart = new Part { PartNumber = "NEW 002" },
                                   NewPart = new Part { PartNumber = "NEW 002" },
                                   BomChanges = new List<BomChange>
                                                    {
                                                        new BomChange
                                                            {
                                                                ChangeId = 7,
                                                                ChangeState = "PROPOS",
                                                                BomId = 1,
                                                                BomName = "TEST",
                                                                AddedBomDetails = new List<BomDetail>
                                                                    {
                                                                        new BomDetail
                                                                            {
                                                                                DetailId = 1,
                                                                                Qty = 1,
                                                                                PartNumber = "NEW 002"
                                                                            }
                                                                    }
                                                            }
                                                    }
                               };

            this.action = () => this.Sut.AddBomDetail("TEST", this.request, 7004, 1);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage("NEW 002 already being added to BOM!");
        }
    }
}
