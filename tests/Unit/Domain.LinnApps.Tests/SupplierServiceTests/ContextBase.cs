﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ISupplierService Sut { get; set; }

        protected IAuthorisationService MockAuthorisationService { get; set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; set; }

        protected IRepository<Currency, string> MockCurrencyRepository { get; set; }

        protected IRepository<PartCategory, string> MockPartCategoryRepository { get; set; }

        protected IRepository<SupplierOrderHoldHistoryEntry, int> MockSupplierOrderHoldHistory { get; set; }

        protected IRepository<FullAddress, int> MockFullAddressRepository { get; set; }

        protected IRepository<Address, int> MockAddressRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

        protected IRepository<VendorManager, string> VendorManagerRepository { get; set; }

        protected IRepository<Planner, int> PlannerRepository { get; set; }

        protected IRepository<Person, int> PersonRepository { get; set; }

        protected IRepository<SupplierContact, int> SupplierContactRepository { get; set; }

        protected IRepository<SupplierGroup, int> GroupRepository { get; set; }

        protected IRepository<Organisation, int> OrgRepository { get; set; }

        protected ISupplierPack SupplierPack { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockAuthorisationService = Substitute.For<IAuthorisationService>();
            this.MockSupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.MockCurrencyRepository = Substitute.For<IRepository<Currency, string>>();
            this.MockPartCategoryRepository = Substitute.For<IRepository<PartCategory, string>>();
            this.MockSupplierOrderHoldHistory = Substitute.For<IRepository<SupplierOrderHoldHistoryEntry, int>>();
            this.MockFullAddressRepository = Substitute.For<IRepository<FullAddress, int>>();
            this.MockAddressRepository = Substitute.For<IRepository<Address, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.GroupRepository = Substitute.For<IRepository<SupplierGroup, int>>();
            this.VendorManagerRepository = Substitute.For<IRepository<VendorManager, string>>();
            this.PlannerRepository = Substitute.For<IRepository<Planner, int>>();
            this.PersonRepository = Substitute.For<IRepository<Person, int>>();
            this.SupplierContactRepository = Substitute.For<IRepository<SupplierContact, int>>();
            this.OrgRepository = Substitute.For<IRepository<Organisation, int>>();
            this.SupplierPack = Substitute.For<ISupplierPack>();
        }
    }
}
