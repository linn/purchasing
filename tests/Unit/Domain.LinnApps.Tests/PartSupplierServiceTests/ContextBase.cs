﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPartSupplierService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IRepository<Currency, string> CurrencyRepository { get; private set; }

        protected IRepository<OrderMethod, string> OrderMethodRepository { get; private set; }

        protected IRepository<FullAddress, int> AddressRepository { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<Manufacturer, string> ManufacturerRepository { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IRepository<Supplier, int> SupplierRepository { get; private set; }

        protected IRepository<PartSupplier, PartSupplierKey> PartSupplierRepository { get; private set; }

        protected IPartHistoryService PartHistory { get; private set; }

        protected IRepository<PriceChangeReason, string> ChangeReasonsRepository { get; private set; }

        protected IRepository<PreferredSupplierChange, PreferredSupplierChangeKey> PreferredSupplierChangeRepository
        {
            get; private set;
        }

        protected IQueryRepository<StockLocator> StockLocatorRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.CurrencyRepository = Substitute.For<IRepository<Currency, string>>();
            this.OrderMethodRepository = Substitute.For<IRepository<OrderMethod, string>>();
            this.AddressRepository = Substitute.For<IRepository<FullAddress, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.ManufacturerRepository = Substitute.For<IRepository<Manufacturer, string>>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.SupplierRepository = Substitute.For<IRepository<Supplier, int>>();
            this.PartSupplierRepository = Substitute.For<IRepository<PartSupplier, PartSupplierKey>>();
            this.PartHistory = Substitute.For<IPartHistoryService>();
            this.ChangeReasonsRepository = Substitute.For<IRepository<PriceChangeReason, string>>();
            this.PreferredSupplierChangeRepository =
                Substitute.For<IRepository<PreferredSupplierChange, PreferredSupplierChangeKey>>();
            this.StockLocatorRepository = Substitute.For<IQueryRepository<StockLocator>>();
            this.Sut = new PartSupplierService(
                this.MockAuthService,
                this.CurrencyRepository,
                this.OrderMethodRepository,
                this.AddressRepository,
                this.EmployeeRepository,
                this.ManufacturerRepository,
                this.PartRepository,
                this.SupplierRepository,
                this.PartSupplierRepository,
                this.PartHistory,
                this.ChangeReasonsRepository,
                this.PreferredSupplierChangeRepository,
                this.StockLocatorRepository);
        }
    }
}
