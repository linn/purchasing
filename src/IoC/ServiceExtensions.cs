namespace Linn.Purchasing.IoC
{
    using System.Collections.Generic;
    using System.Net.Http;

    using Amazon.Extensions.NETCore.Setup;
    using Amazon.SimpleEmail;

    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Proxy;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            return services.AddTransient<IBuilder<SigningLimit>, SigningLimitResourceBuilder>()
                .AddTransient<IBuilder<PartSupplier>, PartSupplierResourceBuilder>()
                .AddTransient<IBuilder<Supplier>, SupplierResourceBuilder>()
                .AddTransient<IBuilder<Currency>, CurrencyResourceBuilder>()
                .AddTransient<IBuilder<OrderMethod>, OrderMethodResourceBuilder>()
                .AddTransient<IBuilder<LinnDeliveryAddress>, LinnDeliveryAddressResourceBuilder>()
                .AddTransient<IBuilder<UnitOfMeasure>, UnitOfMeasureResourceBuilder>()
                .AddTransient<IBuilder<PackagingGroup>, PackagingGroupResourceBuilder>()
                .AddTransient<IBuilder<Tariff>, TariffResourceBuilder>()
                .AddTransient<IBuilder<Manufacturer>, ManufacturerResourceBuilder>()
                .AddTransient<IBuilder<ResultsModel>, ResultsModelResourceBuilder>()
                .AddTransient<IBuilder<PreferredSupplierChange>, PreferredSupplierChangeResourceBuilder>()
                .AddTransient<IBuilder<PriceChangeReason>, PriceChangeReasonResourceBuilder>()
                .AddTransient<IBuilder<PartCategory>, PartCategoryResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrder>, PurchaseOrderResourceBuilder>()
                .AddTransient<IBuilder<ResultsModel>, ResultsModelResourceBuilder>()
                .AddTransient<IBuilder<VendorManager>, VendorManagerResourceBuilder>();
        }

        public static IServiceCollection AddFacades(this IServiceCollection services)
        {
            return services
                .AddTransient<IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource>, SigningLimitFacadeService>()
                .AddTransient<IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>, PartSupplierFacadeService>()
                .AddTransient<IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey>, PreferredSupplierChangeService>()
                .AddTransient<IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource>, SupplierFacadeService>()
                .AddTransient<ISupplierHoldService, SupplierHoldService>()
                .AddTransient<IPartService, PartService>()
                .AddTransient<IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource>, OrderMethodService>()
                .AddTransient<IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>, CurrencyFacadeService>()
                .AddTransient<IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource>, LinnDeliveryAddressService>()
                .AddTransient<IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>, UnitsOfMeasureService>()
                .AddTransient<IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>, PackagingGroupService>()
                .AddTransient<IFacadeResourceService<Tariff, int, TariffResource, TariffResource>, TariffService>()
                .AddTransient<IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource>, ManufacturerFacadeService>()
                .AddTransient<IFacadeResourceFilterService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>, PurchaseOrderFacadeService>()
                .AddTransient<IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource>, PriceChangeReasonService>()
                .AddTransient<IFacadeResourceService<PartCategory, string, PartCategoryResource, PartCategoryResource>, PartCategoriesService>()
                .AddTransient<IPurchaseOrderReportFacadeService, PurchaseOrderReportFacadeService>()
                .AddTransient<IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource>, VendorManagerFacadeService>()
                .AddTransient<ISpendsReportFacadeService, SpendsReportFacadeService>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IPartSupplierService, PartSupplierService>()
                .AddTransient<ISupplierService, SupplierService>()
                .AddTransient<IAmazonSimpleEmailService>(
                    x => new AmazonSimpleEmailServiceClient(x.GetService<AWSOptions>()?.Region))
                .AddTransient<IEmailService>(x => new EmailService(x.GetService<IAmazonSimpleEmailService>()))
                .AddTransient<ITemplateEngine, TemplateEngine>().AddTransient<IPdfService>(
                    x => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()))
                .AddTransient<IReportingHelper, ReportingHelper>()
                .AddTransient<IPurchaseOrdersReportService, PurchaseOrdersReportService>()
                .AddTransient<IPurchaseOrderService, PurchaseOrderService>()
                .AddTransient<IAuthorisationService, AuthorisationService>()
                .AddTransient<IDatabaseService, DatabaseService>()
                .AddTransient<ISpendsReportService, SpendsReportService>()

            //external services
                .AddTransient<IPurchaseOrdersPack, PurchaseOrdersPack>()
                .AddTransient<IAutocostPack, AutocostPack>()
                .AddTransient<ICurrencyPack, CurrencyPack>()
                .AddTransient<IPurchaseLedgerPack, PurchaseLedgerPack>()
                ;
        }
    }
}
