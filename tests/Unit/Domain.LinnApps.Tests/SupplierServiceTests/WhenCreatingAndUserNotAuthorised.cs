namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndUserNotAuthorised : UnauthorisedContext
    {
        private Action action;

        private Supplier candidate;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new Supplier
                               {
                                   Name = "S",
                                   SupplierId = 1,
                               };
           
            this.MockAuthorisationService.HasPermissionFor(AuthorisedAction.SupplierCreate, Arg.Any<IEnumerable<string>>())
                .Returns(false);

            this.action = () => this.Sut.CreateSupplier(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
