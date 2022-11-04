namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomTypeWithInvalidBomTypeChange : ContextBase
    {
        private Action action;

        private BomTypeChange bomTypeChange;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.ChangeBomType, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var part = new Part
                           {
                               PartNumber = "TEST 001",
                               BomType = "C"
                           };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);

            this.bomTypeChange = new BomTypeChange
                                     {
                                         PartNumber = "TEST 001",
                                         OldBomType = "C",
                                         NewBomType = "Z"
                                     };


            this.action = () => this.Sut.ChangeBomType(this.bomTypeChange, new List<string>());
        }

        [Test]
        public void ShouldThrowInvalidBomTypeChangeException()
        {
            this.action.Should().Throw<InvalidBomTypeChangeException>().WithMessage("Invalid bom type change from C to Z");
        }
    }
}
