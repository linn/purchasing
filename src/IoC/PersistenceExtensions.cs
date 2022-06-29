namespace Linn.Purchasing.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Persistence.LinnApps;
    using Linn.Purchasing.Persistence.LinnApps.Repositories;
    using Linn.Purchasing.Persistence.LinnApps.Repositories.MiniOrder;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services.AddScoped<ServiceDbContext>().AddTransient<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddTransient<ITransactionManager, TransactionManager>()
                .AddTransient<IRepository<SigningLimit, int>, SigningLimitRepository>()
                .AddTransient<IRepository<SigningLimitLog, int>, EntityFrameworkRepository<SigningLimitLog, int>>(
                    r => new EntityFrameworkRepository<SigningLimitLog, int>(r.GetService<ServiceDbContext>()?.SigningLimitLogs))
                .AddTransient<IRepository<PartSupplier, PartSupplierKey>, PartSupplierRepository>()
                    .AddTransient<IQueryRepository<Part>, PartRepository>()
                .AddTransient<IRepository<Supplier, int>, SupplierRepository>()
                .AddTransient<IRepository<Currency, string>, EntityFrameworkRepository<Currency, string>>(r
                    => new EntityFrameworkRepository<Currency, string>(r.GetService<ServiceDbContext>()?.Currencies))
                .AddTransient<IRepository<OrderMethod, string>, EntityFrameworkRepository<OrderMethod, string>>(r
                    => new EntityFrameworkRepository<OrderMethod, string>(r.GetService<ServiceDbContext>()?.OrderMethods))
                .AddTransient<IRepository<LinnDeliveryAddress, int>, LinnDeliveryAddressRepository>()
                .AddTransient<IRepository<UnitOfMeasure, string>, EntityFrameworkRepository<UnitOfMeasure, string>>(r
                    => new EntityFrameworkRepository<UnitOfMeasure, string>(r.GetService<ServiceDbContext>()?.UnitsOfMeasure))
                .AddTransient<IRepository<PackagingGroup, int>, EntityFrameworkRepository<PackagingGroup, int>>(r
                    => new EntityFrameworkRepository<PackagingGroup, int>(r.GetService<ServiceDbContext>()?.PackagingGroups))
                .AddTransient<IRepository<Tariff, int>, EntityFrameworkRepository<Tariff, int>>(r
                    => new EntityFrameworkRepository<Tariff, int>(r.GetService<ServiceDbContext>()?.Tariffs))
                .AddTransient<IRepository<Manufacturer, string>, EntityFrameworkRepository<Manufacturer, string>>(r
                    => new EntityFrameworkRepository<Manufacturer, string>(r.GetService<ServiceDbContext>()?.Manufacturers))
                .AddTransient<IRepository<FullAddress, int>, EntityFrameworkRepository<FullAddress, int>>(r
                    => new EntityFrameworkRepository<FullAddress, int>(r.GetService<ServiceDbContext>()?.FullAddresses))
                .AddTransient<IRepository<PurchaseOrder, int>, PurchaseOrderRepository>()
                .AddTransient<IRepository<MiniOrder, int>, MiniOrderRepository>()
                .AddTransient<IRepository<PurchaseLedger, int>, PurchaseLedgerRepository>()
                .AddTransient<IRepository<Employee, int>, EmployeeRepository>()
                .AddTransient<IRepository<PreferredSupplierChange, PreferredSupplierChangeKey>, PreferredSupplierChangeRepository>()
                .AddTransient<IRepository<PriceChangeReason, string>, EntityFrameworkRepository<PriceChangeReason, string>>(r
                    => new EntityFrameworkRepository<PriceChangeReason, string>(r.GetService<ServiceDbContext>()?.PriceChangeReasons))
                .AddTransient<IRepository<PartHistoryEntry, PartHistoryEntryKey>, EntityFrameworkRepository<PartHistoryEntry, PartHistoryEntryKey>>(r
                    => new EntityFrameworkRepository<PartHistoryEntry, PartHistoryEntryKey>(r.GetService<ServiceDbContext>()?.PartHistory))
                .AddTransient<IRepository<PartCategory, string>, EntityFrameworkRepository<PartCategory, string>>(r
                    => new EntityFrameworkRepository<PartCategory, string>(r.GetService<ServiceDbContext>()?.PartCategories))
                .AddTransient<IRepository<SupplierOrderHoldHistoryEntry, int>, EntityFrameworkRepository<SupplierOrderHoldHistoryEntry, int>>(r
                    => new EntityFrameworkRepository<SupplierOrderHoldHistoryEntry, int>(r.GetService<ServiceDbContext>()?.SupplierOrderHoldHistories))
                .AddTransient<IRepository<Country, string>, EntityFrameworkRepository<Country, string>>(r
                    => new EntityFrameworkRepository<Country, string>(r.GetService<ServiceDbContext>()?.Countries))
                .AddTransient<IRepository<Address, int>, AddressRepository>()
                .AddTransient<IRepository<VendorManager, string>, VendorManagerRepository>()
                .AddTransient<IRepository<Planner, int>, PlannerRepository>()
                .AddTransient<IQueryRepository<SupplierSpend>, SupplierSpendRepository>()
                .AddTransient<IQueryRepository<UnacknowledgedOrders>, EntityFrameworkQueryRepository<UnacknowledgedOrders>>(r
                    => new EntityFrameworkQueryRepository<UnacknowledgedOrders>(r.GetService<ServiceDbContext>()?.UnacknowledgedOrders))
                .AddTransient<IQueryRepository<SuppliersWithUnacknowledgedOrders>, EntityFrameworkQueryRepository<SuppliersWithUnacknowledgedOrders>>(r
                    => new EntityFrameworkQueryRepository<SuppliersWithUnacknowledgedOrders>(r.GetService<ServiceDbContext>()?.SuppliersWithUnacknowledgedOrders))
                .AddTransient<IQueryRepository<SupplierGroupsWithUnacknowledgedOrders>, EntityFrameworkQueryRepository<SupplierGroupsWithUnacknowledgedOrders>>(r
                    => new EntityFrameworkQueryRepository<SupplierGroupsWithUnacknowledgedOrders>(r.GetService<ServiceDbContext>()?.SupplierGroupsWithUnacknowledgedOrders))
                .AddTransient<IRepository<SupplierGroup, int>, EntityFrameworkRepository<SupplierGroup, int>>(
                    r => new EntityFrameworkRepository<SupplierGroup, int>(r.GetService<ServiceDbContext>()?.SupplierGroups))
                .AddTransient<IRepository<SupplierContact, int>, SupplierContactRepository>()
                .AddTransient<IRepository<Person, int>, EntityFrameworkRepository<Person, int>>(
                r => new EntityFrameworkRepository<Person, int>(r.GetService<ServiceDbContext>()?.Persons))
                .AddTransient<IRepository<PlCreditDebitNote, int>, PlCreditDebitNoteRepository>()
                .AddTransient<IRepository<PurchaseOrderReq, int>, PurchaseOrderReqRepository>()
                .AddTransient<IRepository<Organisation, int>, EntityFrameworkRepository<Organisation, int>>(r
                    => new EntityFrameworkRepository<Organisation, int>(r.GetService<ServiceDbContext>()?.Organisations))
                .AddTransient<IRepository<TqmsJobref, string>, EntityFrameworkRepository<TqmsJobref, string>>(r
                    => new EntityFrameworkRepository<TqmsJobref, string>(r.GetService<ServiceDbContext>()?.TqmsJobrefs))
                .AddTransient<IQueryRepository<PartReceivedRecord>, EntityFrameworkQueryRepository<PartReceivedRecord>>(
                    r => new EntityFrameworkQueryRepository<PartReceivedRecord>(r.GetService<ServiceDbContext>()
                        ?.TqmsView))
                .AddTransient<IPurchaseOrderDeliveryRepository, PurchaseOrderDeliveryRepository>()
                .AddTransient<IRepository<PurchaseOrderReqState, string>, EntityFrameworkRepository<PurchaseOrderReqState, string>>(
                    r => new EntityFrameworkRepository<PurchaseOrderReqState, string>(r.GetService<ServiceDbContext>()?.PurchaseOrderReqStates))
                .AddTransient<IRepository<OverbookAllowedByLog, int>, OverbookAllowedByLogRespository>()
                .AddTransient<IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>, EntityFrameworkRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>>(
                    r => new EntityFrameworkRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey>(r.GetService<ServiceDbContext>()?.PurchaseOrderReqStateChanges))
                .AddTransient<IRepository<MrpRunLog, int>, EntityFrameworkRepository<MrpRunLog, int>>(
                    r => new EntityFrameworkRepository<MrpRunLog, int>(r.GetService<ServiceDbContext>()?.MrpRunLogs))
                .AddTransient<IWhatsInInspectionRepository, WhatsInInspectionRepository>()
                .AddTransient<IQueryRepository<WhatsInInspectionPurchaseOrdersData>, EntityFrameworkQueryRepository<WhatsInInspectionPurchaseOrdersData>>(
                    r => new EntityFrameworkQueryRepository<WhatsInInspectionPurchaseOrdersData>(r.GetService<ServiceDbContext>()
                        ?.WhatsInInspectionPurchaseOrdersView))
                .AddTransient<IQueryRepository<ReceiptPrefSupDiff>, ReceiptPrefSupDiffRepository>()
                .AddTransient<IQueryRepository<WhatsInInspectionStockLocationsData>, EntityFrameworkQueryRepository<WhatsInInspectionStockLocationsData>>(
                    r => new EntityFrameworkQueryRepository<WhatsInInspectionStockLocationsData>(r.GetService<ServiceDbContext>()
                        ?.WhatsInInspectionStockLocationsView))
                .AddTransient<IQueryRepository<WhatsInInspectionBackOrderData>, EntityFrameworkQueryRepository<WhatsInInspectionBackOrderData>>(
                    r => new EntityFrameworkQueryRepository<WhatsInInspectionBackOrderData>(r.GetService<ServiceDbContext>()
                        ?.WhatsInInspectionBackOrderView))
                .AddTransient<ISingleRecordRepository<MrMaster>, EntityFrameworkSingleRecordRepository<MrMaster>>(
                    r => new EntityFrameworkSingleRecordRepository<MrMaster>(r.GetService<ServiceDbContext>()?.MrMaster))
                .AddTransient<IRepository<LinnWeek, int>, EntityFrameworkRepository<LinnWeek, int>>(r
                    => new EntityFrameworkRepository<LinnWeek, int>(r.GetService<ServiceDbContext>()?.LinnWeeks))
                .AddTransient<IRepository<LedgerPeriod, int>, EntityFrameworkRepository<LedgerPeriod, int>>(r
                    => new EntityFrameworkRepository<LedgerPeriod, int>(r.GetService<ServiceDbContext>()?.LedgerPeriods))
                .AddTransient<IRepository<EdiOrder, int>, EntityFrameworkRepository<EdiOrder, int>>(r
                    => new EntityFrameworkRepository<EdiOrder, int>(r.GetService<ServiceDbContext>()?.EdiOrders))
                .AddTransient<IRepository<CancelledOrderDetail, int>, EntityFrameworkRepository<CancelledOrderDetail, int>>(
                    r => new EntityFrameworkRepository<CancelledOrderDetail, int>(r.GetService<ServiceDbContext>()?.CancelledPurchaseOrderDetails))
                .AddTransient<IQueryRepository<StockLocator>, EntityFrameworkQueryRepository<StockLocator>>(
                    r => new EntityFrameworkQueryRepository<StockLocator>(r.GetService<ServiceDbContext>()
                        ?.StockLocators))
                .AddTransient<IQueryRepository<MrUsedOnRecord>, EntityFrameworkQueryRepository<MrUsedOnRecord>>(
                    r => new EntityFrameworkQueryRepository<MrUsedOnRecord>(r.GetService<ServiceDbContext>()
                        ?.MrUsedOnView))
                .AddTransient<IQueryRepository<PartAndAssembly>, EntityFrameworkQueryRepository<PartAndAssembly>>(
                    r => new EntityFrameworkQueryRepository<PartAndAssembly>(r.GetService<ServiceDbContext>()?.PartsAndAssemblies))
                .AddTransient<IRepository<RescheduleReason, string>, EntityFrameworkRepository<RescheduleReason, string>>(
                    r => new EntityFrameworkRepository<RescheduleReason, string>(r.GetService<ServiceDbContext>()?.PlRescheduleReasons))
                .AddTransient<IQueryRepository<MrHeader>, MrHeaderRepository>()
                .AddTransient<IQueryRepository<MrPurchaseOrderDetail>, MrPurchaseOrderRepository>()
                .AddTransient<ISingleRecordRepository<PurchaseLedgerMaster>, EntityFrameworkSingleRecordRepository<PurchaseLedgerMaster>>(
                    r => new EntityFrameworkSingleRecordRepository<PurchaseLedgerMaster>(r.GetService<ServiceDbContext>()?.PurchaseLedgerMaster))
                .AddTransient<IRepository<MiniOrderDelivery, MiniOrderDeliveryKey>, EntityFrameworkRepository<MiniOrderDelivery, MiniOrderDeliveryKey>>(
                    r => new EntityFrameworkRepository<MiniOrderDelivery, MiniOrderDeliveryKey>(r.GetService<ServiceDbContext>()?.MiniOrdersDeliveries))
                .AddTransient<IQueryRepository<ShortagesEntry>, EntityFrameworkQueryRepository<ShortagesEntry>>(
                    r => new EntityFrameworkQueryRepository<ShortagesEntry>(r.GetService<ServiceDbContext>()
                        ?.ShortagesEntries))
                .AddTransient<IRepository<PartNumberList, string>, EntityFrameworkRepository<PartNumberList, string>>(
                    r => new EntityFrameworkRepository<PartNumberList, string>(r.GetService<ServiceDbContext>()?.PartNumberLists));
        }
    }
}
