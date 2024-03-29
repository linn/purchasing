﻿namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
            PartSupplierFacadeService
        {
            get; private set;
        }

        protected IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey,
            PreferredSupplierChangeResource, PreferredSupplierChangeKey> PreferredSupplierChangeService
        {
            get; private set;
        }

        protected ILog Log { get; private set; }

        protected IPartFacadeService PartFacadeService { get; private set; }

        protected IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> SupplierFacadeService
        {
            get;
            private set;
        }

        protected IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource,
            PriceChangeReasonResource> PriceChangeReasonService
        {
            get;
            private set;
        }

        protected IFacadeResourceService<PartCategory, string, PartCategoryResource,
            PartCategoryResource> PartCategoryService
        {
            get;
            private set;
        }

        protected IRepository<PartCategory, string> MockPartCategoriesRepository { get; set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; private set; }

        protected ISupplierService MockDomainService { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected ISupplierHoldService SupplierHoldService { get; set; }

        protected IDatabaseService MockDatabaseService { get; set; }

        protected IFacadeResourceService<Planner, int, PlannerResource, PlannerResource> PlannerService
        {
            get;
            private set;
        }

        protected IRepository<Planner, int> MockPlannerRepository { get; private set; }

        protected IRepository<Employee, int> MockEmployeeRepository { get; private set; }

        protected IRepository<PartSupplier, PartSupplierKey> MockPartSupplierRepository { get; private set; }

        protected IPartSupplierService MockPartSupplierDomainService { get; set; }

        protected IBulkLeadTimesUpdaterService BulkLeadTimesUpdaterService { get; set; }

        protected IRepository<FullAddress, int> MockFullAddressRepository { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();

            this.MockAuthService = Substitute.For<IAuthorisationService>();

            this.MockPartSupplierRepository = Substitute.For<IRepository<PartSupplier, PartSupplierKey>>();

            this.MockPartSupplierDomainService = Substitute.For<IPartSupplierService>();

            this.MockFullAddressRepository = Substitute.For<IRepository<FullAddress, int>>();

            this.PartSupplierFacadeService = new PartSupplierFacadeService(
                this.MockPartSupplierRepository,
                this.TransactionManager,
                new PartSupplierResourceBuilder(this.MockAuthService),
                this.MockPartSupplierDomainService);

                this.PartFacadeService = Substitute.For<IPartFacadeService>();

            this.Log = Substitute.For<ILog>();

            this.MockSupplierRepository = Substitute.For<IRepository<Supplier, int>>();

            this.MockDomainService = Substitute.For<ISupplierService>();

            this.MockDatabaseService = Substitute.For<IDatabaseService>();

            this.SupplierFacadeService = new SupplierFacadeService(
                this.MockSupplierRepository,
                this.TransactionManager,
                new SupplierResourceBuilder(this.MockAuthService, new SupplierContactResourceBuilder(), new AddressResourceBuilder(MockFullAddressRepository)),
                this.MockDomainService,
                this.MockDatabaseService);

                this.PreferredSupplierChangeService = Substitute
                .For<IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey>>();

                this.PriceChangeReasonService = Substitute
                .For<IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource>>();

            this.MockPartCategoriesRepository = Substitute.For<IRepository<PartCategory, string>>();

            this.PartCategoryService = new PartCategoriesService(
                this.MockPartCategoriesRepository,
                this.TransactionManager,
                new PartCategoryResourceBuilder());

            this.SupplierHoldService = new SupplierHoldService(
                this.MockDomainService,
                this.MockDatabaseService,
                this.TransactionManager,
                new SupplierResourceBuilder(this.MockAuthService, new SupplierContactResourceBuilder(), new AddressResourceBuilder(MockFullAddressRepository)));

            this.MockPlannerRepository = Substitute.For<IRepository<Planner, int>>();
            this.MockEmployeeRepository = Substitute.For<IRepository<Employee, int>>();

            this.PlannerService = new PlannerService(
                this.MockPlannerRepository,
                this.TransactionManager,
                new PlannerResourceBuilder(this.MockEmployeeRepository));

            this.BulkLeadTimesUpdaterService = new BulkLeadTimesUpdaterService(
                this.MockPartSupplierDomainService,
                this.TransactionManager);

            this.Client = TestClient.With<SupplierModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.PartSupplierFacadeService);
                        services.AddSingleton(this.Log);
                        services.AddSingleton(this.PartFacadeService);
                        services.AddSingleton(this.SupplierFacadeService);
                        services.AddSingleton(this.PreferredSupplierChangeService);
                        services.AddSingleton(this.PriceChangeReasonService);
                        services.AddSingleton(this.PartCategoryService);
                        services.AddSingleton(this.SupplierHoldService);
                        services.AddSingleton(this.PlannerService);
                        services.AddSingleton(this.BulkLeadTimesUpdaterService);

                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
