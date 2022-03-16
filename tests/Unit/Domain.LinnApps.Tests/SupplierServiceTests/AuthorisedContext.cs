namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class AuthorisedContext : ContextBase
    {
        [SetUp]
        public void SetUpAuthorised()
        {
            this.MockAuthorisationService.HasPermissionFor(
                    Arg.Is<string>(
                        x => x.Equals(AuthorisedAction.SupplierCreate) 
                             || x.Equals(AuthorisedAction.SupplierUpdate)
                             || x.Equals(AuthorisedAction.SupplierHoldChange)), 
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.Sut = new SupplierService(
                this.MockAuthorisationService,
                this.MockSupplierRepository,
                this.MockCurrencyRepository,
                this.MockPartCategoryRepository,
                this.MockSupplierOrderHoldHistory,
                this.MockAddressRepository,
                this.EmployeeRepository,
                this.VendorManagerRepository,
                this.PlannerRepository,
                this.PersonRepository,
                this.SupplierContactRepository,
                this.GroupRepository,
                this.OrgRepository);
        }
    }
}
