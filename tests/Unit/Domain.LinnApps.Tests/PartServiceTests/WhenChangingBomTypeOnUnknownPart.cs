namespace Linn.Purchasing.Domain.LinnApps.Tests.PartServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenChangingBomTypeOnUnknownPart : ContextBase
    {
        private Action action;

        private BomTypeChange bomTypeChange;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.ChangeBomType, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.bomTypeChange = new BomTypeChange
                                     {
                                         PartNumber = "TEST 001",
                                         NewBomType = "A"
                                     };


            this.action = () => this.Sut.ChangeBomType(this.bomTypeChange, new List<string>());
        }

        [Test]
        public void ShouldThrowItemNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
