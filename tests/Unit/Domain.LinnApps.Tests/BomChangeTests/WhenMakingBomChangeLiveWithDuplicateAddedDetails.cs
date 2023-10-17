namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;

    using NUnit.Framework;

    public class WhenMakingLiveBomChangeWithDuplicateAddedDetails : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT",
                               AddedBomDetails = new List<BomDetail>
                                                     {
                                                         new BomDetail { PartNumber = "PART A" },
                                                         new BomDetail { PartNumber = "PART A" },
                                                         new BomDetail { PartNumber = "PART B" },
                                                         new BomDetail { PartNumber = "PART B" }
                                                     }
                           };
            var employee = new Employee { Id = 1, FullName = "Piers Morgan" };
            this.action = () => this.Sut.MakeLive(employee);
        }

        [Test]
        public void ShouldNotBeLive()
        {
            this.action.Should().Throw<InvalidBomChangeException>().WithMessage("Can't add duplicate bom details: PART A, PART B, ");
            this.Sut.ChangeState.Should().NotBe("LIVE");
            this.Sut.DateApplied.Should().BeNull();
            this.Sut.AppliedBy.Should().BeNull();
            this.Sut.AppliedById.Should().BeNull();
        }
    }
}
