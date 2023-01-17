namespace Linn.Purchasing.Domain.LinnApps.Tests.BomVerificationHistoryServiceTests
{
    using System;
    using System.Linq.Expressions;
    using FluentAssertions;
    using Linn.Common.Domain.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCreatingBomHistoryVerificationEntryButInvalidPart : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns((Part)null);
            this.action = () => this.Sut.ValidPartNumber("FAKE TEST PART");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.action.Should().Throw<DomainException>();
        }
    }
}
