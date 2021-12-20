﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPartSupplierService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IRepository<Currency, string> CurrencyRepository { get; private set; }

        protected IRepository<OrderMethod, string> OrderMethodRepository { get; private set; }

        protected IRepository<Address, int> AddressRepository { get; private set; }

        protected IRepository<Tariff, int> TariffRepository { get; private set; }

        protected IRepository<PackagingGroup, int> PackagingGroupRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.CurrencyRepository = Substitute.For<IRepository<Currency, string>>();
            this.OrderMethodRepository = Substitute.For<IRepository<OrderMethod, string>>();
            this.AddressRepository = Substitute.For<IRepository<Address, int>>();
            this.PackagingGroupRepository = Substitute.For<IRepository<PackagingGroup, int>>();
            this.TariffRepository = Substitute.For<IRepository<Tariff, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();

            this.Sut = new PartSupplierService(
                this.MockAuthService,
                this.CurrencyRepository,
                this.OrderMethodRepository,
                this.AddressRepository,
                this.TariffRepository,
                this.PackagingGroupRepository,
                this.EmployeeRepository);
        }
    }
}
