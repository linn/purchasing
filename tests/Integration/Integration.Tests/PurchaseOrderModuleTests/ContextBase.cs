﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net.Http;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> CurrencyService { get; private set; }

        protected IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> DeliveryAddressService { get; private set; }

        protected ILog Log { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IDatabaseService MockDatabaseService { get; private set; }

        protected IRepository<PurchaseOrderReq, int> MockPurchaseOrderReqRepository { get; private set; }

        protected IRepository<PurchaseOrder, int> MockPurchaseOrderRepository { get; private set; }

        protected IRepository<FullAddress, int> MockFullAddressRepository { get; private set; }

        protected IRepository<OverbookAllowedByLog, int> OverbookAllowedByLogRepository { get; private set; }

        protected IPurchaseOrderReqService MockReqDomainService { get; private set; }

        protected IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> OrderMethodService { get; private set; }

        protected IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource> PackagingGroupService { get; private set; }

        protected IPurchaseOrderFacadeService PurchaseOrderFacadeService { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<Tariff, int, TariffResource, TariffResource> TariffService { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource> UnitsOfMeasureService { get; private set; }

        protected IPurchaseOrderService MockDomainService { get; private set; }

        protected IRepository<NominalAccount, int> MockNominalAccountRepository { get; private set; }

        protected IRepository<Supplier, int> MockSupplierRepository { get; private set; }

        protected IPlCreditDebitNoteService MockPlCreditDebitNoteService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.CurrencyService =
                Substitute.For<IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>>();
            this.OrderMethodService = Substitute
                .For<IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource>>();
            this.DeliveryAddressService = Substitute
                .For<IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource>>();
            this.UnitsOfMeasureService = Substitute
                .For<IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>>();
            this.PackagingGroupService = Substitute
                .For<IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>>();
            this.TariffService = Substitute.For<IFacadeResourceService<Tariff, int, TariffResource, TariffResource>>();
            this.MockReqDomainService = Substitute.For<IPurchaseOrderReqService>();
            this.MockDomainService = Substitute.For<IPurchaseOrderService>();

            this.MockPurchaseOrderReqRepository = Substitute.For<IRepository<PurchaseOrderReq, int>>();
            this.MockPurchaseOrderRepository = Substitute.For<IRepository<PurchaseOrder, int>>();
            this.MockFullAddressRepository = Substitute.For<IRepository<FullAddress, int>>();
            this.OverbookAllowedByLogRepository = Substitute.For<IRepository<OverbookAllowedByLog, int>>();
            this.MockNominalAccountRepository = Substitute.For<IRepository<NominalAccount, int>>();
            this.MockSupplierRepository = Substitute.For<IRepository<Supplier, int>>();

            this.MockDatabaseService = Substitute.For<IDatabaseService>();
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockPlCreditDebitNoteService = Substitute.For<IPlCreditDebitNoteService>();

            var purchaseOrderResourceBuilder = new PurchaseOrderResourceBuilder(
                this.MockAuthService,
                new PurchaseOrderDetailResourceBuilder(
                    new PurchaseOrderDeliveryResourceBuilder(),
                    new PurchaseOrderPostingResourceBuilder(this.MockNominalAccountRepository),
                    this.MockAuthService),
                new LinnDeliveryAddressResourceBuilder(),
                new AddressResourceBuilder(this.MockFullAddressRepository),
                new SupplierResourceBuilder(this.MockAuthService, new SupplierContactResourceBuilder(), new AddressResourceBuilder(this.MockFullAddressRepository)));

            this.Log = Substitute.For<ILog>();

            this.PurchaseOrderFacadeService = new PurchaseOrderFacadeService(
                this.MockPurchaseOrderRepository,
                this.TransactionManager,
                purchaseOrderResourceBuilder,
                this.MockDomainService,
                this.OverbookAllowedByLogRepository,
                this.MockPlCreditDebitNoteService,
                this.Log);

            this.Client = TestClient.With<PurchaseOrderModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.PurchaseOrderFacadeService);
                        services.AddSingleton(this.CurrencyService);
                        services.AddSingleton(this.OrderMethodService);
                        services.AddSingleton(this.DeliveryAddressService);
                        services.AddSingleton(this.UnitsOfMeasureService);
                        services.AddSingleton(this.PackagingGroupService);
                        services.AddSingleton(this.TariffService);
                        services.AddSingleton(this.Log);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
