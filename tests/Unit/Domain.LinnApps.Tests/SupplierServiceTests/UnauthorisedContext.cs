namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class UnauthorisedContext : ContextBase
    {
        [SetUp]
        public void SetUpAuthorised()
        {
            this.MockAuthorisationService.HasPermissionFor(
                    Arg.Is<string>(
                        x => x.Equals(AuthorisedAction.SupplierCreate) || x.Equals(AuthorisedAction.SupplierUpdate)),
                    Arg.Any<IEnumerable<string>>())
                .Returns(false);
            this.Sut = new SupplierService(
                this.MockAuthorisationService,
                this.MockSupplierRepository,
                this.MockCurrencyRepository,
                this.MockPartCategoryRepository,
                this.MockSupplierOrderHoldHistory);
        }
    }
}
