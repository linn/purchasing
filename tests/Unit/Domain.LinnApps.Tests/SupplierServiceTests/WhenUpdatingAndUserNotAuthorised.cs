namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NUnit.Framework;

    public class WhenUpdatingAndUserNotAuthorised : UnauthorisedContext
    {
        private Action action;

        private Supplier current;

        private Supplier updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new Supplier
                               {
                                   SupplierId = 1,
                                   Name = string.Empty
                               };
            this.updated = new Supplier
                               {
                                   SupplierId = 1,
                                   Name = "We updated this to this."
                               };
            this.action = () => this.Sut.UpdateSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.current.Name.Should().Be(string.Empty);
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
