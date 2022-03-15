namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBulkUpdatingLeadTimesAndNotAuthorised : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.action = () => this.Sut.BulkUpdateLeadTimes(
                1, new List<LeadTimeUpdateModel>(), new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.PartSupplierRepository.DidNotReceive()
                .FindBy(Arg.Any<Expression<Func<PartSupplier, bool>>>());
        }
    }
}
