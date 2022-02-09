namespace Linn.Purchasing.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Persistence.LinnApps;
    using Linn.Purchasing.Persistence.LinnApps.Repositories;

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
                .AddTransient<IRepository<PurchaseLedger, int>, PurchaseLedgerRepository>()
                .AddTransient<IRepository<Employee, int>, EntityFrameworkRepository<Employee, int>>(r
                    => new EntityFrameworkRepository<Employee, int>(r.GetService<ServiceDbContext>()?.Employees))
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
                .AddTransient<IRepository<Address, int>, EntityFrameworkRepository<Address, int>>(r
                    => new EntityFrameworkRepository<Address, int>(r.GetService<ServiceDbContext>()?.Addresses));
        }
    }
}
