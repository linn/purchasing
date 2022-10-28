namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenChangingBomTypeAndNotAuthorised : ContextBase
    {
        private Action action;

        private BomTypeChange bomTypeChange;

        [SetUp]
        public void SetUp()
        {
            this.bomTypeChange = new BomTypeChange
                                     {
                                         PartNumber = "TEST 001",
                                         OldBomType = "C",
                                         OldSupplierId = 1,
                                         NewBomType = "A",
                                         NewSupplierId = 2
            };


            this.action = () => this.Sut.ChangeBomType(this.bomTypeChange, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
