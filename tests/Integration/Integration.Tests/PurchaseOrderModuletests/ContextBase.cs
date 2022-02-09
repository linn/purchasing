﻿namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
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

        protected IFacadeResourceFilterService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
           PurchaseOrderFacadeService
        {
            get; private set;
        }

        protected IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> CurrencyService { get; private set; }

        protected IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> OrderMethodService { get; private set; }

        protected IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> 
            DeliveryAddressService
        {
            get; private set;
        }

        protected IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>
            UnitsOfMeasureService
        {
            get; private set;
        }

        protected IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>
            PackagingGroupService
        {
            get;
            private set;
        }

        protected IFacadeResourceService<Tariff, int, TariffResource, TariffResource> TariffService
        {
            get;
            private set;
        }

        protected ILog Log { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.PurchaseOrderFacadeService =
                Substitute
                    .For<IFacadeResourceFilterService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>>();
            this.CurrencyService = Substitute.For<IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>>();
            this.OrderMethodService = Substitute
                .For<IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource>>();
            this.DeliveryAddressService = Substitute
                .For<IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource>>();
            this.UnitsOfMeasureService = Substitute
                .For<IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>>();
            this.PackagingGroupService = Substitute
                .For<IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>>();
            this.TariffService = Substitute.For<IFacadeResourceService<Tariff, int, TariffResource, TariffResource>>();
            this.Log = Substitute.For<ILog>();

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
