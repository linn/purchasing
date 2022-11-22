namespace Linn.Purchasing.IoC
{
    using System.Net.Http;

    using Amazon.Extensions.NETCore.Setup;
    using Amazon.SimpleEmail;

    using Linn.Common.Authorisation;
    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Proxy;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Resources.SearchResources;

    using Microsoft.Extensions.DependencyInjection;

    using RazorEngineCore;

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
                .AddTransient<IReportReturnResourceBuilder, ReportReturnResourceBuilder>()
                .AddTransient<IBuilder<PreferredSupplierChange>, PreferredSupplierChangeResourceBuilder>()
                .AddTransient<IBuilder<PriceChangeReason>, PriceChangeReasonResourceBuilder>()
                .AddTransient<IBuilder<PartCategory>, PartCategoryResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderDelivery>, PurchaseOrderDeliveryResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderPosting>, PurchaseOrderPostingResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderDetail>, PurchaseOrderDetailResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrder>, PurchaseOrderResourceBuilder>()
                .AddTransient<IBuilder<Address>, AddressResourceBuilder>()
                .AddTransient<IBuilder<Country>, CountryResourceBuilder>()
                .AddTransient<IBuilder<VendorManager>, VendorManagerResourceBuilder>()
                .AddTransient<IBuilder<Planner>, PlannerResourceBuilder>()
                .AddTransient<IBuilder<SupplierGroup>, SupplierGroupResourceBuilder>()
                .AddTransient<IBuilder<SupplierContact>, SupplierContactResourceBuilder>()
                .AddTransient<IBuilder<PlCreditDebitNote>, PlCreditDebitNoteResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderReq>, PurchaseOrderReqResourceBuilder>()
                .AddTransient<IBuilder<MrpRunLog>, MrpRunLogResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderReqState>, PurchaseOrderReqStateResourceBuilder>()
                .AddTransient<IBuilder<MrMaster>, MrMasterResourceBuilder>()
                .AddTransient<IBuilder<EdiOrder>, EdiOrderResourceBuilder>()
                .AddTransient<IBuilder<PurchaseOrderDelivery>, PurchaseOrderDeliveryResourceBuilder>()
                .AddTransient<IBuilder<MrReport>, MrReportResourceBuilder>()
                .AddTransient<IBuilder<MrPurchaseOrderDetail>, MrPurchaseOrderResourceBuilder>()
                .AddTransient<IBuilder<MrReportOptions>, MrReportOptionsResourceBuilder>()
                .AddTransient<IBuilder<EdiSupplier>, EdiSupplierResourceBuilder>()
                .AddTransient<IBuilder<AutomaticPurchaseOrder>, AutomaticPurchaseOrderResourceBuilder>()
                .AddTransient<IBuilder<AutomaticPurchaseOrderSuggestion>, AutomaticPurchaseOrderSuggestionResourceBuilder>()
                .AddTransient<IBuilder<LedgerPeriod>, LedgerPeriodResourceBuilder>()
                .AddTransient<IBuilder<ChangeRequest>, ChangeRequestResourceBuilder>()
                .AddTransient<IBuilder<BomChange>, BomChangeResourceBuilder>()
                .AddTransient<IBuilder<Bom>, BomResourceBuilder>()
                .AddTransient<IBuilder<PcasChange>, PcasChangeResourceBuilder>()
                .AddTransient<IBuilder<BomTypeChange>, BomTypeChangeResourceBuilder>()
                .AddTransient<IBuilder<CircuitBoard>, CircuitBoardResourceBuilder>();
        }

        public static IServiceCollection AddFacades(this IServiceCollection services)
        {
            return services
                .AddTransient<IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource>, SigningLimitFacadeService>()
                .AddTransient<IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>, PartSupplierFacadeService>()
                .AddTransient<IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey>, PreferredSupplierChangeService>()
                .AddTransient<IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource>, SupplierFacadeService>()
                .AddTransient<ISupplierHoldService, SupplierHoldService>()
                .AddTransient<IPartFacadeService, PartFacadeService>()
                .AddTransient<IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource>, OrderMethodService>()
                .AddTransient<IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>, CurrencyFacadeService>()
                .AddTransient<IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource>, LinnDeliveryAddressService>()
                .AddTransient<IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>, UnitsOfMeasureService>()
                .AddTransient<IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>, PackagingGroupService>()
                .AddTransient<IFacadeResourceService<Tariff, int, TariffResource, TariffResource>, TariffService>()
                .AddTransient<IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource>, ManufacturerFacadeService>()
                .AddTransient<IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource>, PriceChangeReasonService>()
                .AddTransient<IFacadeResourceService<PartCategory, string, PartCategoryResource, PartCategoryResource>, PartCategoriesService>()
                .AddTransient<IPurchaseOrderReportFacadeService, PurchaseOrderReportFacadeService>()
                .AddTransient<IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource>, AddressService>()
                .AddTransient<IFacadeResourceService<Country, string, CountryResource, CountryResource>, CountryService>()
                .AddTransient<IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource>, VendorManagerFacadeService>()
                .AddTransient<ISpendsReportFacadeService, SpendsReportFacadeService>()
                .AddTransient<IFacadeResourceService<Planner, int, PlannerResource, PlannerResource>, PlannerService>()
                .AddTransient<IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource>, SupplierGroupFacadeService>()
                .AddTransient<IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource>, PlCreditDebitNoteFacadeService>()
                .AddTransient<IPlCreditDebitNoteEmailService, PlCreditDebitNoteEmailService>()
                .AddTransient<IBulkLeadTimesUpdaterService, BulkLeadTimesUpdaterService>()
                .AddTransient<ITqmsJobRefService, TqmsJobRefService>()
                .AddTransient<IPartsReceivedReportFacadeService, PartsReceivedReportFacadeService>()
                .AddTransient<ISpendsReportFacadeService, SpendsReportFacadeService>()
                .AddTransient<IPurchaseOrderReqFacadeService, PurchaseOrderReqFacadeService>()
                .AddTransient<IWhatsDueInReportFacadeService, WhatsDueInReportFacadeService>()
                .AddTransient<IFacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource>, PurchaseOrderReqStateFacadeService>()
                .AddTransient<IOutstandingPoReqsReportFacadeService, OutstandingPoReqsReportFacadeService>()
                .AddTransient<IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource>, MrpRunLogFacadeService>()
                .AddTransient<IMaterialRequirementsPlanningFacadeService, MaterialRequirementsPlanningFacadeService>()
                .AddTransient<IWhatsInInspectionReportFacadeService, WhatsInInspectionReportFacadeService>()
                .AddTransient<IPrefSupReceiptsReportFacadeService, PrefSupReceiptsReportFacadeService>()
                .AddTransient<ISingleRecordFacadeResourceService<MrMaster, MrMasterResource>, MrMasterFacadeService>()
                .AddTransient<IForecastingFacadeService, ForecastingFacadeService>()
                .AddTransient<IEdiOrdersFacadeService, EdiOrdersFacadeService>()
                .AddTransient<IMrUsedOnReportFacadeService, MrUsedOnReportFacadeService>()
                .AddTransient<IPurchaseOrderDeliveryFacadeService, PurchaseOrderDeliveryFacadeService>()
                .AddTransient<IMaterialRequirementsReportFacadeService, MaterialRequirementsReportFacadeService>()
                .AddTransient<IShortagesReportFacadeService, ShortagesReportFacadeService>()
                .AddTransient<IMrOrderBookReportFacadeService, MrOrderBookReportFacadeService>()
                .AddTransient<IPurchaseOrderFacadeService, PurchaseOrderFacadeService>()
                .AddTransient<IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource>, AutomaticPurchaseOrderFacadeService>()
                .AddTransient<IFacadeResourceFilterService<AutomaticPurchaseOrderSuggestion, int, AutomaticPurchaseOrderSuggestionResource, AutomaticPurchaseOrderSuggestionResource, PlannerSupplierRequestResource>, AutomaticPurchaseOrderSuggestionFacadeService>()
                .AddTransient<ILeadTimesReportFacadeService, LeadTimesReportFacadeService>()
                .AddTransient<IForecastingReportsFacadeService, ForecastingReportsFacadeService>()
                .AddTransient<IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource>, LedgerPeriodFacadeService>()
                .AddTransient<IForecastWeekChangesFacadeService, ForecastWeekChangesFacadeService>()
                .AddTransient<IChangeRequestFacadeService, ChangeRequestFacadeService>()
                .AddTransient<IFacadeResourceService<Bom, int, BomResource, BomResource>, BomFacadeService>()
                .AddTransient<IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource>, CircuitBoardFacadeService>()
                .AddTransient<IBomTreeReportsService, BomTreeReportService>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<IPartSupplierService, PartSupplierService>()
                .AddTransient<ISupplierService, SupplierService>()
                .AddTransient<IAmazonSimpleEmailService>(x => new AmazonSimpleEmailServiceClient(x.GetService<AWSOptions>()?.Region))
                .AddTransient<IEmailService>(x => new EmailService(x.GetService<IAmazonSimpleEmailService>()))
                .AddTransient<IPdfService>(x => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()))
                .AddTransient<IReportingHelper, ReportingHelper>()
                .AddTransient<IPurchaseOrdersReportService, PurchaseOrdersReportService>()
                .AddTransient<IPurchaseOrderService, PurchaseOrderService>()
                .AddTransient<IAuthorisationService, AuthorisationService>()
                .AddTransient<IDatabaseService, DatabaseService>()
                .AddTransient<ISpendsReportService, SpendsReportService>()
                .AddTransient<IPlCreditDebitNoteService, PlCreditDebitNoteService>()
                .AddTransient<IPartsReceivedReportService, PartsReceivedReportService>()
                .AddTransient<IPurchaseOrderReqService>(
                    x => new PurchaseOrderReqService(
                        ConfigurationManager.Configuration["APP_ROOT"],
                        x.GetService<IAuthorisationService>(),
                        x.GetService<IPurchaseOrderReqsPack>(),
                        x.GetService<IRepository<Employee, int>>(),
                        x.GetService<IEmailService>(),
                        x.GetService<IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>>(),
                        x.GetService<IPurchaseOrderAutoOrderPack>(),
                        x.GetService<IPurchaseOrdersPack>(),
                        x.GetService<ICurrencyPack>(),
                        x.GetService<IQueryRepository<Part>>(),
                        x.GetService<IRepository<Supplier, int>>()))
                .AddTransient<IWhatsDueInReportService, WhatsDueInReportService>()
                .AddTransient<IOutstandingPoReqsReportService, OutstandingPoReqsReportService>()
                .AddTransient<IMaterialRequirementsPlanningService, MaterialRequirementsPlanningService>()
                .AddTransient<IWhatsInInspectionReportService, WhatsInInspectionReportService>()
                .AddTransient<IPrefSupReceiptsReportService, PrefSupReceiptsReportService>()
                .AddTransient<IForecastingService, ForecastingService>()
                .AddTransient<IEdiOrderService, EdiOrderService>()
                .AddTransient<IMrUsedOnReportService, MrUsedOnReportService>()
                .AddTransient<IPurchaseOrderDeliveryService, PurchaseOrderDeliveryService>()
                .AddTransient<IMaterialRequirementsReportService, MaterialRequirementsReportService>()
                .AddTransient<IShortagesReportService, ShortagesReportService>()
                .AddTransient<IMrOrderBookReportService, MrOrderBookReportService>()
                .AddTransient<IAutomaticPurchaseOrderService, AutomaticPurchaseOrderService>()
                .AddTransient<ISupplierKitService, SupplierKitService>()
                .AddTransient<ILeadTimesReportService, LeadTimesReportService>()
                .AddTransient<ISupplierAutoEmailsMailer, SupplierAutoEmailsMailer>()
                .AddTransient<IForecastOrdersReportService, ForecastOrdersReportService>()
                .AddTransient<IDeliveryPerformanceReportService, DeliveryPerformanceReportService>()
                .AddTransient<IForecastWeekChangesReportService, ForecastWeekChangesReportService>()
                .AddTransient<IPartService, PartService>()
                .AddTransient<IPartHistoryService, PartHistoryService>()
                .AddTransient<IBomTreeService, BomTreeService>()
                .AddTransient<IChangeRequestService, ChangeRequestService>()

                // external services
                .AddTransient<IPurchaseOrdersPack, PurchaseOrdersPack>()
                .AddTransient<IAutocostPack, AutocostPack>()
                .AddTransient<ICurrencyPack, CurrencyPack>()
                .AddTransient<ILedgerPeriodPack, LedgerPeriodPack>()
                .AddTransient<IPurchaseLedgerPack, PurchaseLedgerPack>()
                .AddTransient<IPurchaseOrderReqsPack, PurchaseOrderReqsPack>()
                .AddTransient<IMrpLoadPack, MrpLoadPack>()
                .AddTransient<IForecastingPack, ForecastingPack>()
                .AddTransient<IEdiEmailPack, EdiEmailPack>()
                .AddTransient<ISupplierPack, SupplierPack>()
                .AddTransient<IPurchaseOrderAutoOrderPack, PurchaseOrderAutoOrderPack>()
                .AddTransient<IRazorEngine, RazorEngine>()
                .AddTransient<ITemplateEngine, RazorTemplateEngine>()
                .AddTransient<ISalesTaxPack, SalesTaxPack>()

                // proxy
                .AddTransient<IHtmlTemplateService<PurchaseOrder>>(x =>
                    new HtmlTemplateService<PurchaseOrder>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}PurchaseOrder.cshtml",
                        x.GetService<ITemplateEngine>()))
                .AddTransient<IHtmlTemplateService<PlCreditDebitNote>>(x =>
                    new HtmlTemplateService<PlCreditDebitNote>(
                        $"{ConfigurationManager.Configuration["VIEWS_ROOT"]}PlCreditDebitNote.cshtml",
                        x.GetService<ITemplateEngine>()));
        }
    }
}
