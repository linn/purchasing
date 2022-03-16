namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    using NUnit.Framework;

    public class WhenCreatingPreferredSupplierAndUnauthorised : ContextBase
    {
        private Action action;

        private PreferredSupplierChange candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PreferredSupplierChange
                                 {
                                 };


            this.action = () => this.Sut.CreatePreferredSupplierChange(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
