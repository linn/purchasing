﻿namespace Linn.Purchasing.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Facade.Carter;
    using Linn.Common.Facade.Carter.Handlers;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Service.ResultHandlers;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.AddTransient<UniversalResponseNegotiator>()
                .AddTransient<IHandler, JsonResultHandler<PartSupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PartSupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<SupplierResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddTransient<IHandler, JsonResultHandler<ProcessResultResourceWithLinks>>()
                .AddTransient<IHandler, JsonResultHandler<SigningLimitResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SigningLimitResource>>>()
                .AddTransient<IHandler, SigningLimitApplicationStateResultHandler>()
                .AddTransient<IHandler, PartSupplierApplicationStateResultHandler>()
                .AddTransient<IHandler, CircuitBoardApplicationStateResultHandler>()
                .AddTransient<IHandler, SuppliersApplicationStateResultHandler>()
                .AddTransient<IHandler, JsonResultHandler<CurrencyResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<CurrencyResource>>>()
                .AddTransient<IHandler, JsonResultHandler<OrderMethodResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<OrderMethodResource>>>()
                .AddTransient<IHandler, JsonResultHandler<LinnDeliveryAddressResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<LinnDeliveryAddressResource>>>()
                .AddTransient<IHandler, JsonResultHandler<UnitOfMeasureResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<UnitOfMeasureResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PackagingGroupResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PackagingGroupResource>>>()
                .AddTransient<IHandler, JsonResultHandler<TariffResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<TariffResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ManufacturerResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ManufacturerResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PreferredSupplierChangeResource>>()
                .AddTransient<IHandler, JsonResultHandler<PriceChangeReasonResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PriceChangeReasonResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PartPriceConversionsResource>>()
                .AddTransient<IHandler, JsonResultHandler<PartCategoryResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PartCategoryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PurchaseOrderResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PurchaseOrderResource>>>()
                .AddTransient<IHandler, JsonResultHandler<AddressResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<AddressResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<CountryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<VendorManagerResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<VendorManagerResource>>>()
                .AddTransient<IHandler, JsonResultHandler<SupplierGroupResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<SupplierGroupResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PlannerResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PlCreditDebitNoteResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<TqmsJobRefResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PlCreditDebitNoteResource>>()
                .AddTransient<IHandler, JsonResultHandler<PurchaseOrderReqResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PurchaseOrderReqResource>>>()
                .AddTransient<IHandler, PurchaseOrderApplicationStateResultHandler>()
                .AddTransient<IHandler, PurchaseOrderReqApplicationStateResultHandler>()
                .AddTransient<IHandler, JsonResultHandler<PurchaseOrderReqStateResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PurchaseOrderReqStateResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<MrpRunLogResource>>>()
                .AddTransient<IHandler, JsonResultHandler<MrpRunLogResource>>()
                .AddTransient<IHandler, JsonResultHandler<WhatsInInspectionReportResource>>()
                .AddTransient<IHandler, JsonResultHandler<MrMasterResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<EdiOrderResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PurchaseOrderDeliveryResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PurchaseOrderDeliveryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<BatchUpdateProcessResultResourceWithLinks>>()
                .AddTransient<IHandler, JsonResultHandler<MrReportResource>>()
                .AddTransient<IHandler, JsonResultHandler<MrPurchaseOrdersResource>>()
                .AddTransient<IHandler, JsonResultHandler<MrReportOptionsResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<EdiSupplierResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ReportReturnResource>>>()
                .AddTransient<IHandler, JsonResultHandler<AutomaticPurchaseOrderResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<AutomaticPurchaseOrderResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<AutomaticPurchaseOrderSuggestionResource>>>()
                .AddTransient<IHandler, JsonResultHandler<LedgerPeriodResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<LedgerPeriodResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ChangeRequestResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ChangeRequestResource>>>()
                .AddTransient<IHandler, JsonResultHandler<BomTypeChangeResource>>()
                .AddTransient<IHandler, JsonResultHandler<CircuitBoardResource>>()
                .AddTransient<IHandler, JsonResultHandler<BomTreeNode>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<CircuitBoardResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<BomCostReportResource>>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<BoardComponentSummaryResource>>>()
                .AddTransient<IHandler, JsonResultHandler<BomVerificationHistoryResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<BomVerificationHistoryResource>>>()
                .AddTransient<IHandler, PlCreditDebitNoteApplicationStateResultHandler>()
                .AddTransient<IHandler, JsonResultHandler<BomStandardPricesResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<BomHistoryReportLineResource>>>()
                .AddTransient<IHandler, JsonResultHandler<PartDataSheetValuesResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<PartDataSheetValuesResource>>>()
                .AddTransient<IHandler, CsvResultHandler<IEnumerable<BomTreeNode>>>()
                .AddTransient<IHandler, CsvResultHandler<ReportReturnResource>>()
                .AddTransient<IHandler, CsvResultHandler<IEnumerable<PcasChangeComponent>>>()
                .AddTransient<IHandler, CsvResultHandler<IEnumerable<IEnumerable<string>>>>();
        }
    }
}
