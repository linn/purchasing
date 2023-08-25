namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExplodingOntoAComponent : ContextBase
    {
        private Action action;

        [SetUp]
        public void Setup()
        {
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "COMP", BomType = "C", DecrementRule = "YES" });
            this.action = () => this.Sut.ExplodeSubAssembly("COMP", 123, "BITS", 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage($"COMP is a Component! Can't add to its BOM");
        }
    }
}
